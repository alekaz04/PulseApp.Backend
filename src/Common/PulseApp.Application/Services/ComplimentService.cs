using Microsoft.EntityFrameworkCore;
using PulseApp.Application.Interfaces;
using PulseApp.Authentication.Abstraction;
using PulseApp.Common;
using PulseApp.Domain.Entities;
using PulseApp.Infrastructure;

namespace PulseApp.Application.Services;

/// <inheritdoc/>
public class ComplimentService : IComplimentService
{
    private readonly ICurrentUserService _currentUserService;

    /// <inheritdoc cref="PulseDataContext"/>
    private readonly PulseDataContext _context;

    public ComplimentService(ICurrentUserService currentUserService, PulseDataContext context)
    {
        _currentUserService = currentUserService;
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<Compliment> GetRandomCompliment(CancellationToken token)
    {
        var compliment = await _context.Set<Compliment>()
            .Where(x => x.IsBeenPushed == false && x.IsDeleted == false)
            .OrderBy(o => Guid.NewGuid())
            .Take(1)
            .FirstOrDefaultAsync(token);

        if (compliment == null)
        {
            throw new CommonErrorException("Compliment for push not found");
        }

        compliment.IsBeenPushed = true;

        await _context.SaveChangesAsync(token);
        return compliment;
    }

    /// <inheritdoc/>
    public async Task<int> ResetAllPushedCompliments(CancellationToken token)
    {
        return await _context.Set<Compliment>().Where(x => x.IsBeenPushed)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsBeenPushed, false), token);
    }

    public async Task<Compliment> GetComplimentById(Guid complimentId, CancellationToken token)
    {
        var currentUserId = _currentUserService.CurrentUser?.Id ?? throw new CommonErrorException("User not log in");
        var compliment = await _context.Set<Compliment>()
            .FirstOrDefaultAsync(x => x.Id == complimentId && x.CreatedByUserId == currentUserId, token);

        return compliment ?? throw new CommonErrorException("Compliment not found");
    }

    public async Task SetComplimentAsPushed(Guid complimentId, CancellationToken token)
    {
        var currentUserId = _currentUserService.CurrentUser?.Id ?? throw new CommonErrorException("User not log in");
        var compliment = await _context.Set<Compliment>()
            .FirstOrDefaultAsync(x => x.Id == complimentId && x.CreatedByUserId == currentUserId, token)
                         ?? throw new CommonErrorException("Compliment not found");

        compliment.IsBeenPushed = true;
        await _context.SaveChangesAsync(token);
    }
}
