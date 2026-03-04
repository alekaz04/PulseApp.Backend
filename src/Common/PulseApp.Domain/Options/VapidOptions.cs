namespace PulseApp.Domain.Options;

/// <summary>
/// Конфигурация ключей VAPID
/// </summary>
public class VapidOptions
{
    /// <summary>
    /// Почта автора уведомлений
    /// </summary>
    public string Subject { get; set; } = null!;

    /// <summary>
    /// Публичный ключ
    /// </summary>
    public string PublicKey { get; set; } = null!;

    /// <summary>
    /// Приватный ключ
    /// </summary>
    public string PrivateKey { get; set; } = null!;
}
