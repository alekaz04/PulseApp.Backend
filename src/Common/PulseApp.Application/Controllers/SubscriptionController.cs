// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseApp.Application.Interfaces;

namespace PulseApp.Application.Controllers;

[ApiController]
[Route("api/subscription")]
[Authorize]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionHandler _handler;

    public SubscriptionController(ISubscriptionHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateSubscriptionCode(CancellationToken token)
    {
        string code = await _handler.CreateSubscriptionCode(token);
        return Ok(code);
    }

    /// <summary>
    /// Получить всех подписчиков пользователя
    /// </summary>
    /// <param name="token">Токен отмены запроса</param>
    [HttpGet]
    public async Task<IActionResult> GetAllSubscribers(CancellationToken token)
    {
        var result = await _handler.GetAllSubscriptionForUser(token);
        return Ok(result);
    }

    /// <summary>
    /// Отправить комплимент пользователю
    /// </summary>
    [HttpPost("push/to/{subscriptionId:guid}")]
    public async Task<ActionResult> SendComplimentToUser([FromRoute] Guid subscriptionId, [FromQuery] Guid complimentId, CancellationToken token)
    {
        await _handler.SendComplimentToUser(subscriptionId, complimentId, token);

        return Ok();
    }
}
