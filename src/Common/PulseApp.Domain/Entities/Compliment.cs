namespace PulseApp.Domain.Entities;

/// <summary>
/// Сущность комплимента
/// </summary>
public class Compliment
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Текст
    /// </summary>
    public string Text { get; set; } = null!;

    /// <summary>
    /// Заголовок
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Флаг удаления
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Флаг уже отправленного комплимента
    /// </summary>
    public bool IsBeenPushed { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}
