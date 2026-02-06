using PulseApp.Application.DTOs;
using PulseApp.Domain.Entities;

namespace PulseApp.Application.Interfaces;

/// <summary>
/// Сервис для отправки push-уведомлений
/// </summary>
public interface IPushNotificationService
{
    /// <summary>
    /// Отправить push-уведомление одному подписчику
    /// </summary>
    Task SendNotificationToSubscriber(SubscriptionPush subscription, PushNotificationPayload payload, CancellationToken token);

    /// <summary>
    /// Отправить push-уведомление всем подписчикам
    /// </summary>
    Task SendNotificationToAllAsync(PushNotificationPayload payload, CancellationToken token);

    /// <summary>
    /// Отправить push-уведомление подписчикам
    /// </summary>
    Task SendNotificationToSubscribers(List<SubscriptionPush> subscriptions, PushNotificationPayload payload, CancellationToken token);
}
