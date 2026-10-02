using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PulseApp.Application.Controllers;

[ApiController]
[Authorize]
[Route("api/code")]
public class SubscriptionCodeController(ISubscriptionCodeHandler handler) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateSubscriptionCode([FromBody] CreateSubscriptionCodeDto createCodeDto, CancellationToken token)
    {
        string code = await handler.CreateSubscriptionCode(createCodeDto, token);
        return Ok(code);
    }
}
