using PulseApp.Application.DTOs;

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
}
