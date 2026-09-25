using PulseApp.Application.Controllers;
using PulseApp.Application.Interfaces.Services;
using PulseApp.Authentication.Abstraction;

namespace PulseApp.Application.Handlers;

public class SubscriptionCodeHandler : ISubscriptionCodeHandler
{
    private readonly ISubscriptionCodeService _subscriptionCodeService;

    public SubscriptionCodeHandler(ISubscriptionCodeService subscriptionCodeService)
    {
        _subscriptionCodeService = subscriptionCodeService;
    }

    public async Task<string> CreateSubscriptionCode(CreateSubscriptionCodeDto createCodeDto, CancellationToken token)
    {
        return await _subscriptionCodeService.CreateSubscriptionCode(createCodeDto, token);
    }
}
