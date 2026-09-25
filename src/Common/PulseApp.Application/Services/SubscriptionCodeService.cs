using PulseApp.Application.Controllers;
using PulseApp.Application.Interfaces.Services;
using PulseApp.Authentication.Abstraction;
using PulseApp.Domain.Entities;
using PulseApp.Infrastructure;

namespace PulseApp.Application.Services;

public class SubscriptionCodeService : ISubscriptionCodeService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly PulseDataContext _context;

    public SubscriptionCodeService(ICurrentUserService currentUserService, PulseDataContext context)
    {
        _currentUserService = currentUserService;
        _context = context;
    }

    public async Task<string> CreateSubscriptionCode(CreateSubscriptionCodeDto createCodeDto, CancellationToken token)
    {
        var createUserId = _currentUserService.GetCurrentUserId();
        var codeObj = new SubscriptionCode()
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString(),
            Name = createCodeDto.Name,
            CreatedAt = DateTimeOffset.UtcNow,
            ExpireAt = DateTimeOffset.UtcNow.AddHours(24),
            IsUsed = false,
            CreatedCodeUserId = createUserId,
        };
        await _context.AddAsync(codeObj, token);
        await _context.SaveChangesAsync(token);

        return codeObj.Code;
    }
}
