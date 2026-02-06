// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using PulseApp.Application.Interfaces;
using PulseApp.Domain.Entities;

namespace PulseApp.Infrastructure.Services;

/// <summary>
/// Реализация сервиса для работы с подписками на push-уведомления
/// </summary>
public class SubscriptionService : ISubscriptionService
{
    private readonly PulseDataContext _context;

    public SubscriptionService(PulseDataContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateOrUpdateSubscriptionAsync(
        string endpoint,
        string p256dh,
        string auth,
        string? userAgent)
    {
        var existing = await _context.PushSubscriptions
            .FirstOrDefaultAsync(x => x.Endpoint == endpoint);

        if (existing is not null)
        {
            // Если подписка существует, то обновляем её
            existing.P256dh = p256dh;
            existing.Auth = auth;
            existing.IsActive = true;
            existing.UserAgent = userAgent;

            _context.PushSubscriptions.Update(existing);
            await _context.SaveChangesAsync();

            return existing.Id;
        }

        // Создаем новую подписку
        var subscription = new PushSubscription
        {
            Id = Guid.NewGuid(),
            Endpoint = endpoint,
            P256dh = p256dh,
            Auth = auth,
            UserAgent = userAgent,
            CreatedAt = DateTimeOffset.UtcNow,
            IsActive = true
        };

        _context.PushSubscriptions.Add(subscription);
        await _context.SaveChangesAsync();

        return subscription.Id;
    }

    public async Task DeleteSubscriptionByEndpointAsync(string endpoint)
    {
        var subscription = await _context.PushSubscriptions
            .FirstOrDefaultAsync(x => x.Endpoint == endpoint);

        if (subscription is not null)
        {
            _context.PushSubscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<PushSubscription>> GetActiveSubscriptionsAsync()
    {
        return await _context.PushSubscriptions
            .Where(x => x.IsActive)
            .ToListAsync();
    }

    public async Task DeactivateSubscriptionAsync(Guid id)
    {
        var subscription = await _context.PushSubscriptions
            .FirstOrDefaultAsync(x => x.Id == id);

        if (subscription != null)
        {
            subscription.IsActive = false;
            _context.PushSubscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }
    }
}
