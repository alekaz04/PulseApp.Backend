using Microsoft.Extensions.Logging;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;
using PulseApp.Common;
using PulseApp.Domain.Entities;

namespace PulseApp.Application.Handlers;

/// <inheritdoc/>
public class ComplimentHandler : IComplimentHandler
{
    /// <inheritdoc cref="IComplimentService"/>
    private readonly IComplimentService _service;

    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<ComplimentHandler> _logger;

    public ComplimentHandler(IComplimentService service, ILogger<ComplimentHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Guid> CreateCompliment(CreateComplimentDto complimentDto, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(complimentDto.Text) || string.IsNullOrWhiteSpace(complimentDto.Title))
        {
            throw new CommonErrorException("Text and Title must not be empty");
        }

        var newCompliment = new Compliment()
        {
            Id = Guid.NewGuid(),
            Text = complimentDto.Text,
            Title = complimentDto.Title,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        await _service.CreateCompliment(newCompliment, token);

        _logger.LogInformation("Compliment created");

        return newCompliment.Id;
    }

    /// <inheritdoc/>
    public async Task<List<Guid>> CreateBatchCompliment(List<CreateComplimentDto> complimentDtos, CancellationToken token)
    {
        var compliments = complimentDtos.Select(x => new Compliment()
        {
            Id = Guid.NewGuid(),
            Text = x.Text,
            Title = x.Title,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        }).ToList();

        await _service.CreateBatchCompliment(compliments, token);
        _logger.LogInformation("Batch compliments create is complete");
        return compliments.Select(x => x.Id).ToList();
    }
}
