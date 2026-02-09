using Microsoft.AspNetCore.Mvc;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;

namespace PulseApp.Application.Controllers;

/// <summary>
/// Контроллер для администратора системы
/// </summary>
[ApiController]
[Route("api/admin")]
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
    public async Task<ActionResult<SendNotificationResponse>> SendToAll([FromBody] SendNotificationRequest request, CancellationToken token)
    {
        return await _handler.SendToAll(request, token);
    }
}

