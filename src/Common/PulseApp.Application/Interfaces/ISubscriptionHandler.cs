using PulseApp.Application.DTOs;

namespace PulseApp.Application.Interfaces;

/// <summary>
/// Обработчик подписок на пуш уведомления
/// </summary>
public interface ISubscriptionHandler
{
    /// <summary>
    /// Подписаться на пуш уведомления
    /// </summary>
    /// <param name="subscribeRequest">Запрос на подписку</param>
    /// <param name="token">Токен отмены запросы</param>
    public Task<SubscribeResponse> Subscribe(SubscribeRequest subscribeRequest, CancellationToken token);

    /// <summary>
    /// Отписаться от пуш уведомлений
    /// </summary>
    /// <param name="endpoint">Endpoint подписки</param>
    /// <param name="token">Токен отмены запроса</param>
    public Task<UnsubscribeResponse> Unsubscribe(string endpoint, CancellationToken token);
}
