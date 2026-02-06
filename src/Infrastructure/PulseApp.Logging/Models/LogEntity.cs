namespace PulseApp.Logging.Models;

/// <summary>
/// Сущность записи лога
/// </summary>
public class LogEntity
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Уровень
    /// </summary>
    public string Level { get; set; } = null!;

    /// <summary>
    /// Время
    /// </summary>
    public DateTimeOffset RaiseDate { get; set; }

    /// <summary>
    /// Сообщение
    /// </summary>
    public string Message { get; set; } = null!;

    /// <summary>
    /// Исключение
    /// </summary>
    public string? Exception { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Все свойства лога
    /// </summary>
    public string LogEventProperties { get; set; } = null!;
}
