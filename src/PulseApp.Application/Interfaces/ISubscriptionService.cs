// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using PulseApp.Domain.Entities;

namespace PulseApp.Application.Interfaces;

/// <summary>
/// Сервис для работы с подписками на push-уведомления
/// </summary>
public interface ISubscriptionService
{
    /// <summary>
    /// Создать новую подписку
    /// </summary>
    Task<Guid> CreateSubscriptionAsync(string endpoint, string p256dh, string auth, string? userAgent);

    /// <summary>
    /// Удалить подписку по endpoint
    /// </summary>
    Task DeleteSubscriptionByEndpointAsync(string endpoint);

    /// <summary>
    /// Получить все активные подписки
    /// </summary>
    Task<List<PushSubscription>> GetActiveSubscriptionsAsync();

    /// <summary>
    /// Деактивировать подписку
    /// </summary>
    Task DeactivateSubscriptionAsync(Guid id);
}
