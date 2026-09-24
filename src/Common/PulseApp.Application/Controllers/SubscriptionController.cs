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
}
