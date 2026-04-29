using PulseApp.Application.DTOs;
using PulseApp.Domain.Entities;

namespace PulseApp.Application.Interfaces;

/// <summary>
/// Обработчик административных команд
/// </summary>
public interface IAdministrationHandler
{
    /// <summary>
    /// Отправить пуш уведомление всем подписчикам
    /// </summary>
    /// <param name="request">Запрос на пуш уведомление</param>
    /// <param name="token">Токен отмены запроса</param>
    public Task<SendNotificationResponse> SendToAll(SendNotificationRequest request, CancellationToken token);

    /// <summary>
    /// Получить все активные подписки
    /// </summary>
    /// <param name="token">Токен отмены запроса</param>
    /// <returns>Список активных подписок</returns>
    Task<List<SubscriptionPush>> GetAllSubscriptions(CancellationToken token);
}
