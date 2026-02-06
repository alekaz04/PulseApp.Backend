using System.Diagnostics;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PulseApp.Common.ErrorMiddleware;

/// <summary>
/// Миддлвар для отлова ошибок
/// </summary>
public class ErrorMiddleware
{
    /// <inheritdoc cref="RequestDelegate"/>
    private readonly RequestDelegate _next;

    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<ErrorMiddleware> _logger;

    public ErrorMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (CommonErrorException e)
        {
            _logger.LogError(e, "Handled CommonErrorException: {Message}", e.Message);
            await HandleException(context, e.Message, 400);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error while executing request");
            await HandleException(context, "Internal Server Error", 500);
        }
    }

    /// <summary>
    /// Запаковать ошибку в ответ
    /// </summary>
    private static Task HandleException(HttpContext context, string message, int statusCode)
    {
        var errorResponse = new ErrorResponse
        {
            Message = message,
            TraceId = Activity.Current?.TraceId.ToString()
        };

        string json = JsonSerializer.Serialize(errorResponse);

        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsync(json);
    }
}
