using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PulseApp.Application.DTOs;
using PulseApp.Domain.Options;

namespace PulseApp.Application.Controllers;

/// <summary>
/// Контроллер для получения vapid ключа
/// </summary>
[ApiController]
[Route("api/vapid")]
public class VapidController : ControllerBase
{
    /// <inheritdoc cref="IVapidService"/>
    private readonly IOptions<VapidOptions> _vapidOptions;

    public VapidController(IOptions<VapidOptions> options)
    {
        _vapidOptions = options;
    }

    /// <summary>
    /// Получить VAPID публичный ключ
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(VapidPublicKeyResponse), StatusCodes.Status200OK)]
    public ActionResult<string> GetVapidPublicKey()
    {
        string publicKey = _vapidOptions.Value.PublicKey;
        return Ok(new { PublicKey = publicKey });
    }
}
