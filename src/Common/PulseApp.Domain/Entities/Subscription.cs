namespace PulseApp.Domain.Entities;

/// <summary>
/// Сущность подписки на push-уведомления
/// </summary>
public class Subscription
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Endpoint для отправки push-уведомлений (уникальный)
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Публичный ключ клиента для шифрования (P256DH)
    /// </summary>
    public string P256dh { get; set; } = string.Empty;

    /// <summary>
    /// Auth secret для шифрования
    /// </summary>
    public string Auth { get; set; } = string.Empty;

    /// <summary>
    /// Дата создания подписки
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User Agent браузера (опционально)
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// Флаг активности подписки
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Идентификатор пользователя хозяина
    /// </summary>
    public Guid UserOwnerId { get; set; }

    /// <summary>
    /// Пользователь хозяин
    /// </summary>
    public User UserOwner { get; set; } = null!;
}
