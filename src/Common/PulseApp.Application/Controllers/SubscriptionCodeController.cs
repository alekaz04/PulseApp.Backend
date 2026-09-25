using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PulseApp.Application.Controllers;

[ApiController]
[Authorize]
[Route("api/code")]
public class SubscriptionCodeController : ControllerBase
{
    private readonly ISubscriptionCodeHandler _handler;

    [HttpPost("create")]
    public async Task<IActionResult> CreateSubscriptionCode([FromBody] CreateSubscriptionCodeDto createCodeDto, CancellationToken token)
    {
        string code = await _handler.CreateSubscriptionCode(createCodeDto, token);
        return Ok(code);
    }
}
