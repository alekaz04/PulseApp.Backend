using Microsoft.AspNetCore.Mvc;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;
using PulseApp.Authentication;

namespace PulseApp.Application.Controllers;

/// <summary>
/// Контроллер для администратора системы
/// </summary>
[ApiController]
[Route("api/admin")]
[ApiKey]
public class AdministrationController : ControllerBase
{
    /// <inheritdoc cref="IAdministrationHandler"/>
    private readonly IAdministrationHandler _handler;

    public AdministrationController(IAdministrationHandler handler)
    {
        _handler = handler;
    }

    /// <summary>
    /// Отправить push-уведомление всем подписчикам
    /// </summary>
    [HttpPost("push/all")]
    public async Task<ActionResult> SendToAll([FromBody] SendNotificationRequest request, CancellationToken token)
    {
        var result = await _handler.SendToAll(request, token);

        return Ok(result);
    }

    /// <summary>
    /// Получить список активных подписок
    /// </summary>
    [HttpGet("all")]
    public async Task<ActionResult> GetAllSubscriptions(CancellationToken token)
    {
        var result = await _handler.GetAllSubscriptions(token);
        return Ok(result);
    }

    /// <summary>
    /// Восстановить пул комплиментов
    /// </summary>
    [HttpPost("compliments/reset-pool")]
    public async Task<ActionResult> ResetComplimentPool(CancellationToken token)
    {
        int result = await _handler.ResetComplimentsPool(token);
        return Ok(result);
    }
}

