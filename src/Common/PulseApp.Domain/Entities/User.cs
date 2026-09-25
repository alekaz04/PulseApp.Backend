namespace PulseApp.Domain.Entities;

/// <summary>
/// Сущность пользователя системы
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя в сервисе аутентификации (клейм sub)
    /// </summary>
    public string KeycloakId { get; set; } = null!;

    /// <summary>
    /// Почта. Может отсутствовать, если у пользователя в Keycloak не указан email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string DisplayName { get; set; } = null!;

    /// <summary>
    /// Время создания пользователя
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    public List<SubscriptionPush> Subscriptions { get; set; }
}
