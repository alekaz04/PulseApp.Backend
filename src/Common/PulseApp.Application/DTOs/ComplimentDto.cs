namespace PulseApp.Application.DTOs;

public class ComplimentDto
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

public class CreateComplimentDto
{
    public string Text { get; set; } = null!;

    public string Title { get; set; } = null!;
}
