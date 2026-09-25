using PulseApp.Domain.Entities;

namespace PulseApp.Application.Interfaces;

/// <summary>
/// Сервис для отправки push-уведомлений
/// </summary>
public interface IPushNotificationService
{
    /// <summary>
    /// Отправить комплимент пользователю подписчику
    /// </summary>
    /// <param name="subscription">Подписчик</param>
    /// <param name="compliment">Комплимент</param>
    /// <param name="token">Токен отмены запроса</param>
    Task SendComplimentNotification(Subscription subscription, Compliment compliment, CancellationToken token);
}
