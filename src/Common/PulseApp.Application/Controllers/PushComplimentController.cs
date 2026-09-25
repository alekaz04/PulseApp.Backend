using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseApp.Application.Services;

namespace PulseApp.Application.Controllers;

[ApiController]
[Authorize]
[Route("api/compliment")]
public class PushComplimentController(IPushComplimentService service) : ControllerBase
{
    /// <summary>
    /// Отправить комплимент пользователю
    /// </summary>
    [HttpPost("push/to/{subscriptionId:guid}")]
    public async Task<ActionResult> SendComplimentToUser([FromRoute] Guid subscriptionId, [FromQuery] Guid complimentId, CancellationToken token)
    {
        await service.SendComplimentToUser(subscriptionId, complimentId, token);

        return Ok();
    }
}
