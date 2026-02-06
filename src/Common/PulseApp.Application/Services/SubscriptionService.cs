using Microsoft.EntityFrameworkCore;
using PulseApp.Application.Interfaces;
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

    public SubscriptionService(PulseDataContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<Guid> CreateOrUpdateSubscription(string endpoint, string p256dh, string auth, string? userAgent, CancellationToken token)
    {
        var existing = await _context.Set<SubscriptionPush>()
            .FirstOrDefaultAsync(x => x.Endpoint == endpoint, token);

        if (existing is not null)
        {
            // Если подписка существует, то обновляем её
            existing.P256dh = p256dh;
            existing.Auth = auth;
            existing.IsActive = true;
            existing.UserAgent = userAgent;

            _context.Set<SubscriptionPush>().Update(existing);
            await _context.SaveChangesAsync(token);

            return existing.Id;
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
            IsActive = true
        };

        _context.Set<SubscriptionPush>().Add(subscription);
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
        var subscription = await _context.Set<SubscriptionPush>()
            .FirstOrDefaultAsync(x => x.Id == id, token);

        if (subscription is not null)
        {
            subscription.IsActive = false;
            _context.Set<SubscriptionPush>().Update(subscription);
            await _context.SaveChangesAsync(token);
        }
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
        _context.Set<SubscriptionPush>().Update(subscription);
        await _context.SaveChangesAsync(token);

        return true;
    }
}
