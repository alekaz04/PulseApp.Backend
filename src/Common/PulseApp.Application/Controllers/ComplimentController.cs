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
[ApiKey]
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
    public async Task<IActionResult> CreateCompliment([FromBody] CreateComplimentDto complimentDto, CancellationToken token)
    {
        var result = await _handler.CreateCompliment(complimentDto, token);

        return Ok(result);
    }

    /// <summary>
    /// Создать комплимент
    /// </summary>
    [HttpPost("batch")]
    public async Task<IActionResult> CreateBatchCompliment([FromBody] List<CreateComplimentDto> complimentDtos,
        CancellationToken token)
    {
        var result = await _handler.CreateBatchCompliment(complimentDtos, token);

        return Ok(result);
    }

    /// <summary>
    /// Получить комплимент по идентификатору
    /// </summary>
    [HttpGet("{complimentId:guid}")]
    public async Task<IActionResult> GetCompliment([FromRoute] Guid complimentId, CancellationToken token)
    {
        var result = await _handler.GetComplimentById(complimentId, token);

        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Получить все комплименты
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetCompliments(CancellationToken token)
    {
        var result = await _handler.GetAllCompliments(token);
        return Ok(result);
    }

    /// <summary>
    /// Обновить комплимент по идентификатору
    /// </summary>
    [HttpPut("{complimentId:guid}")]
    public async Task<IActionResult> UpdateCompliment([FromRoute] Guid complimentId, [FromBody] ComplimentUpdateDto complimentUpdateDto, CancellationToken token)
    {
        await _handler.UpdateComplimentById(complimentId, complimentUpdateDto, token);

        return NoContent();
    }

    /// <summary>
    /// Удалить комплимент по идентификатору
    /// </summary>
    [HttpDelete("{complimentId:guid}")]
    public async Task<IActionResult> DeleteCompliment([FromRoute] Guid complimentId, CancellationToken token)
    {
        await _handler.DeleteComplimentById(complimentId, token);
        return NoContent();
    }
}
