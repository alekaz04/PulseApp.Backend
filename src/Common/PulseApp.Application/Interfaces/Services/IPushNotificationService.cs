using PulseApp.Application.DTOs;
using PulseApp.Domain.Entities;
using WebPush;

namespace PulseApp.Application.Interfaces;

/// <summary>
/// Сервис для отправки push-уведомлений
/// </summary>
public interface IPushNotificationService
{
    /// <summary>
    /// Отправить push-уведомление одному подписчику
    /// </summary>
    Task SendNotificationToSubscriber(SubscriptionPush subscription, WebPushClient webPushClient,
        PushNotificationPayload payload, CancellationToken token);

    /// <summary>
    /// Отправить push-уведомление всем подписчикам
    /// </summary>
    Task SendNotificationToAllSubscribes(PushNotificationPayload payload, CancellationToken token);

    /// <summary>
    /// Отправить push-уведомление подписчикам
    /// </summary>
    Task SendNotificationToSubscribers(List<SubscriptionPush> subscriptions, PushNotificationPayload payload, CancellationToken token);
}
