using Microsoft.EntityFrameworkCore;
using PulseApp.Application.Interfaces;
using PulseApp.Authentication.Abstraction;
using PulseApp.Common;
using PulseApp.Domain.Entities;
using PulseApp.Infrastructure;

namespace PulseApp.Application.Services;

/// <summary>
/// Реализация сервиса для работы с подписками на push-уведомления
/// </summary>
public class SubscriptionService : ISubscriptionService
{
    /// <inheritdoc cref="PulseDataContext"/>
    private readonly PulseDataContext _context;

    private readonly ICurrentUserService _currentUserService;


    public SubscriptionService(PulseDataContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    /// <inheritdoc/>
    public async Task<Guid> CreateOrUpdateSubscription(string endpoint, string p256dh, string auth, string? userAgent, string inviteCode ,CancellationToken token)
    {
        var codeUser = await GetCode(inviteCode, token);

        if (codeUser.IsUsed)
        {
            throw new CommonErrorException("Code is already used");
        }

        if (codeUser.ExpireAt < DateTimeOffset.UtcNow)
        {
            throw new CommonErrorException("Expired at is invalid");
        }

        // Создаем новую подписку
        var subscription = new SubscriptionPush
        {
            Id = Guid.NewGuid(),
            Endpoint = endpoint,
            P256dh = p256dh,
            Auth = auth,
            UserAgent = userAgent,
            CreatedAt = DateTimeOffset.UtcNow,
            IsActive = true,
            UserOwnerId = codeUser.CreatedCodeUserId
        };

        _context.Set<SubscriptionPush>().Add(subscription);
        codeUser.IsUsed = true;
        await _context.SaveChangesAsync(token);

        return subscription.Id;
    }

    /// <inheritdoc/>
    public async Task<List<SubscriptionPush>> GetActiveSubscriptions(CancellationToken token)
    {
        return await _context.Set<SubscriptionPush>()
            .Where(x => x.IsActive)
            .AsNoTracking()
            .ToListAsync(token);
    }

    /// <inheritdoc/>
    public async Task DeactivateSubscription(Guid id, CancellationToken token)
    {
        await _context.Set<SubscriptionPush>()
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsActive, false), token);
    }

    /// <inheritdoc/>
    public async Task<bool> DeactivateSubscriptionByEndpoint(string endpoint, CancellationToken token)
    {
        var subscription = await _context.Set<SubscriptionPush>()
            .FirstOrDefaultAsync(x => x.Endpoint == endpoint, token);

        if (subscription is null)
        {
            return false;
        }

        subscription.IsActive = false;
        await _context.SaveChangesAsync(token);

        return true;
    }

    public async Task<string> CreateSubscriptionCode(Guid createUserId, CancellationToken token)
    {
        var codeObj = new SubscriptionCode()
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString(),
            CreatedAt = DateTimeOffset.UtcNow,
            ExpireAt = DateTimeOffset.UtcNow.AddHours(24),
            IsUsed = false,
            CreatedCodeUserId = createUserId,
        };
        await _context.AddAsync(codeObj, token);
        await _context.SaveChangesAsync(token);

        return codeObj.Code;
    }

    public async Task<SubscriptionCode> GetCode(string code, CancellationToken token)
    {
        return await _context.Set<SubscriptionCode>()
            .FirstOrDefaultAsync(x => x.Code == code, token) ?? throw new CommonErrorException($"Код {code} не ю");
    }
}
