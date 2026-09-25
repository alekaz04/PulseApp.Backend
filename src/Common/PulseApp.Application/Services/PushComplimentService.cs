using Microsoft.EntityFrameworkCore;
using PulseApp.Application.Interfaces;
using PulseApp.Authentication.Abstraction;
using PulseApp.Common;
using PulseApp.Domain.Entities;
using PulseApp.Infrastructure;

namespace PulseApp.Application.Services;

public class PushComplimentService(
    ICurrentUserService currentUserService,
    IPushNotificationService pushNotificationService,
    PulseDataContext context) : IPushComplimentService
{
    public async Task SendComplimentToUser(Guid subscriptionId, Guid complimentId, CancellationToken token)
    {
        var currentUserId = currentUserService.GetCurrentUserId();

        var compliment = await context.Set<Compliment>()
            .FirstOrDefaultAsync(x => x.Id == complimentId && !x.IsDeleted && x.CreatedByUserId == currentUserId, token)
                         ?? throw new CommonErrorException($"Комплимент с идентификатором {complimentId} не найден");

        var subscription = await context.Set<Subscription>()
                               .FirstOrDefaultAsync(
                                   x => x.Id == subscriptionId && x.IsActive && x.UserOwnerId == currentUserId, token)
                           ?? throw new CommonErrorException(
                               $"Подписчик с идентификатором {subscriptionId} не найден или не активна");

        await pushNotificationService.SendComplimentNotification(subscription, compliment, token);

        compliment.IsBeenPushed = true;
        await context.SaveChangesAsync(token);
    }
}
