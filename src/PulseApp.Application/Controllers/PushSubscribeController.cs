using Microsoft.AspNetCore.Mvc;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;

namespace PulseApp.Application.Controllers;

/// <summary>
/// Контроллер подписки на пуш уведомления
/// </summary>
[ApiController]
[Route("api/subscribe")]
public class PushSubscribeController : ControllerBase
{
    /// <inheritdoc cref="ISubscriptionHandler"/>
    private readonly ISubscriptionHandler _handler;

    public PushSubscribeController(ISubscriptionHandler handler)
    {
        _handler = handler;
    }

    /// <summary>
    /// Подписаться на push-уведомления
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<SubscribeResponse>> Subscribe([FromBody] SubscribeRequest request, CancellationToken token)
    {
        return await _handler.Subscribe(request, token);
    }
}
