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

    /// <summary>
    /// Отправить комплимент пользователю подписчику
    /// </summary>
    /// <param name="subscription">Подписчик</param>
    /// <param name="compliment">Комплимент</param>
    /// <param name="token">Токен отмены запроса</param>
    Task SendComplimentNotification(SubscriptionPush subscription, Compliment compliment, CancellationToken token);
}
