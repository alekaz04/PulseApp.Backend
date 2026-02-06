using PulseApp.Domain.Options;

namespace PulseApp.Application.Interfaces;

/// <summary>
/// Сервис для работы с VAPID ключами
/// </summary>
public interface IVapidService
{
    /// <summary>
    /// Получить VAPID ключи
    /// </summary>
    VapidOptions GetVapidKeys();

    /// <summary>
    /// Получить публичный ключ
    /// </summary>
    string GetPublicKeyAsync();
}
