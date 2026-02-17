using Microsoft.AspNetCore.Mvc;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;
using PulseApp.Authentication;

namespace PulseApp.Application.Controllers;

/// <summary>
/// Контроллер для администратора системы
/// </summary>
[ApiController]
[Route("api/compliment")]
public class ComplimentController : ControllerBase
{
    /// <inheritdoc cref="IComplimentHandler"/>
    private readonly IComplimentHandler _handler;

    public ComplimentController(IComplimentHandler handler)
    {
        _handler = handler;
    }

    /// <summary>
    /// Создать комплимент
    /// </summary>
    [HttpPost]
    [ApiKey]
    public async Task<IActionResult> CreateCompliment([FromBody] CreateComplimentDto complimentDto,
        CancellationToken token)
    {
        var result = await _handler.CreateCompliment(complimentDto, token);

        return Ok(result);
    }

    /// <summary>
    /// Создать комплимент
    /// </summary>
    [HttpPost("batch")]
    [ApiKey]
    public async Task<IActionResult> CreateBatchCompliment([FromBody] List<CreateComplimentDto> complimentDtos,
        CancellationToken token)
    {
        var result = await _handler.CreateBatchCompliment(complimentDtos, token);

        return Ok(result);
    }
}
