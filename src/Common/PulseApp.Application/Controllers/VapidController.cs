using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;

namespace PulseApp.Application.Controllers;

/// <summary>
/// Контроллер для получения vapid ключа
/// </summary>
[ApiController]
[Route("api/vapid")]
public class VapidController : ControllerBase
{
    /// <inheritdoc cref="IVapidService"/>
    private readonly IVapidService _vapidService;

    public VapidController(IVapidService vapidService)
    {
        _vapidService = vapidService;
    }

    /// <summary>
    /// Получить VAPID публичный ключ
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(VapidPublicKeyResponse), StatusCodes.Status200OK)]
    public ActionResult<string> GetVapidPublicKey()
    {
        string publicKey = _vapidService.GetPublicKeyAsync();
        return Ok(new { PublicKey = publicKey });
    }
}
