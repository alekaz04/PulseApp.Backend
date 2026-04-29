using Microsoft.EntityFrameworkCore;
using PulseApp.Application.Interfaces;
using PulseApp.Common;
using PulseApp.Domain.Entities;
using PulseApp.Infrastructure;

namespace PulseApp.Application.Services;

/// <inheritdoc/>
public class ComplimentService : IComplimentService
{
    /// <inheritdoc cref="PulseDataContext"/>
    private readonly PulseDataContext _context;

    public ComplimentService(PulseDataContext context)
    {
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
}
