namespace PulseApp.Application.DTOs;

/// <summary>
/// Дто обновления комплимента
/// </summary>
public class ComplimentUpdateDto
{
    /// <summary>
    /// Текст
    /// </summary>
    public string Text { get; set; } = null!;

    /// <summary>
    /// Заголовок
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Статус отправки (true - отправлено)
    /// </summary>
    public bool IsBeenPushed { get; set; }
}
