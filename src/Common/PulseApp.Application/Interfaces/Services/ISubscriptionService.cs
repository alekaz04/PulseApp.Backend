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
    Task<Guid> CreateOrUpdateSubscription(string endpoint, string p256dh, string auth, string? userAgent, string inviteCode, CancellationToken token);

    /// <summary>
    /// Получить все активные подписки
    /// </summary>
    Task<List<SubscriptionPush>> GetActiveSubscriptions(CancellationToken token);

    /// <summary>
    /// Деактивировать подписку
    /// </summary>
    Task DeactivateSubscription(Guid id, CancellationToken token);

    /// <summary>
    /// Деактивировать подписку по endpoint
    /// </summary>
    Task<bool> DeactivateSubscriptionByEndpoint(string endpoint, CancellationToken token);

    /// <summary>
    /// Создать код для подписки
    /// </summary>
    /// <param name="createUserId">Пользоатель который создаёт код</param>
    /// <param name="token">Токен отмены запроса</param>
    /// <returns>Код</returns>
    Task<string> CreateSubscriptionCode(Guid createUserId, CancellationToken token);

    /// <summary>
    /// Получить список всех подписок пользователя
    /// </summary>
    /// <param name="token">Токен отмены запросы</param>
    /// <returns>Коллекция подписок</returns>
    Task<List<SubscriptionPush>> GetAllSubscriptionForUser(CancellationToken token);

    /// <summary>
    /// Получить подписку по идентификатору
    /// </summary>
    /// <param name="subscriptionId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<SubscriptionPush> GetSubscriptionById(Guid subscriptionId, CancellationToken token);
}
