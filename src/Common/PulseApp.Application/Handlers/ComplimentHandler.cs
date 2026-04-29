using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;
using PulseApp.Common;
using PulseApp.Domain.Entities;
using PulseApp.Infrastructure;

namespace PulseApp.Application.Handlers;

/// <inheritdoc/>
public class ComplimentHandler : IComplimentHandler
{
    /// <inheritdoc cref="PulseDataContext"/>
    private readonly PulseDataContext _context;

    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<ComplimentHandler> _logger;

    /// <inheritdoc cref="IMapper"/>
    private readonly IMapper _mapper;

    public ComplimentHandler(PulseDataContext context, ILogger<ComplimentHandler> logger, IMapper mapper)
    {
        _context = context;
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

        _context.Add(newCompliment);
        await _context.SaveChangesAsync(token);

        _logger.LogInformation("Compliment created");

        return newCompliment.Id;
    }

    /// <inheritdoc/>
    public async Task<List<Guid>> CreateBatchCompliment(List<CreateComplimentDto> complimentDtos, CancellationToken token)
    {
        var compliments = _mapper.Map<List<Compliment>>(complimentDtos);

        _context.AddRange(compliments);
        await _context.SaveChangesAsync(token);

        _logger.LogInformation("Batch compliments create is complete");
        return compliments.Select(x => x.Id).ToList();
    }

    /// <inheritdoc/>
    public async Task<List<ComplimentDto>> GetAllCompliments(CancellationToken token)
    {
        var compliments = await _context.Set<Compliment>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .ToListAsync(token);

        return _mapper.Map<List<ComplimentDto>>(compliments);
    }

    /// <inheritdoc/>
    public async Task<ComplimentDto?> GetComplimentById(Guid complimentId, CancellationToken token)
    {
        var compliment = await _context.Set<Compliment>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.Id == complimentId)
            .FirstOrDefaultAsync(token);

        return _mapper.Map<ComplimentDto?>(compliment);
    }

    /// <inheritdoc/>
    public async Task UpdateComplimentById(Guid complimentId, ComplimentUpdateDto complimentUpdateDto,
        CancellationToken token)
    {
        var compliment = await _context.Set<Compliment>()
            .Where(x => !x.IsDeleted && x.Id == complimentId)
            .FirstOrDefaultAsync(token);

        _mapper.Map(complimentUpdateDto, compliment);

        _logger.LogInformation("Compliment updated {complimentId}", complimentId);

        await _context.SaveChangesAsync(token);
    }

    /// <inheritdoc/>
    public async Task DeleteComplimentById(Guid complimentId, CancellationToken token)
    {
        var compliment = await _context.Set<Compliment>()
            .Where(x => !x.IsDeleted && x.Id == complimentId)
            .FirstOrDefaultAsync(token);

        if (compliment == null)
        {
            throw new CommonErrorException($"Compliment with id {complimentId} not found");
        }
        compliment.IsDeleted = true;
        _logger.LogInformation("Compliment deleted {complimentId}", complimentId);
        await _context.SaveChangesAsync(token);
    }
}
