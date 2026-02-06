namespace PulseApp.Common.ErrorMiddleware;

/// <summary>
/// Сообщение об ошибке
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Сообщение
    /// </summary>
    public string Message { get; set; } = null!;

    /// <summary>
    /// Идентфикатор запроса
    /// </summary>
    public string? TraceId { get; set; }
}
