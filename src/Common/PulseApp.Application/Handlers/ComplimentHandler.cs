using AutoMapper;
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

    /// <inheritdoc cref="IMapper"/>
    private readonly IMapper _mapper;

    public ComplimentHandler(IComplimentService service, ILogger<ComplimentHandler> logger, IMapper mapper)
    {
        _service = service;
        _logger = logger;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<Guid> CreateCompliment(CreateComplimentDto complimentDto, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(complimentDto.Text) || string.IsNullOrWhiteSpace(complimentDto.Title))
        {
            throw new CommonErrorException("Text and Title must not be empty");
        }

        var newCompliment = _mapper.Map<Compliment>(complimentDto);

        await _service.CreateCompliment(newCompliment, token);

        _logger.LogInformation("Compliment created");

        return newCompliment.Id;
    }

    /// <inheritdoc/>
    public async Task<List<Guid>> CreateBatchCompliment(List<CreateComplimentDto> complimentDtos, CancellationToken token)
    {
        var compliments = _mapper.Map<List<Compliment>>(complimentDtos);

        await _service.CreateBatchCompliment(compliments, token);
        _logger.LogInformation("Batch compliments create is complete");
        return compliments.Select(x => x.Id).ToList();
    }

    /// <inheritdoc/>
    public async Task<List<ComplimentDto>> GetAllCompliments(CancellationToken token)
    {
        var compliments = await _service.GetAllCompliments(token);

        return _mapper.Map<List<ComplimentDto>>(compliments);
    }

    public async Task<ComplimentDto?> GetComplimentById(Guid complimentId, CancellationToken token)
    {
        var compliment = await _service.GetComplimentById(complimentId, token);
        return _mapper.Map<ComplimentDto?>(compliment);
    }
}
