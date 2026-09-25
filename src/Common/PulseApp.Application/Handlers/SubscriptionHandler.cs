using AutoMapper;
using Microsoft.Extensions.Logging;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;
using PulseApp.Authentication.Abstraction;
using PulseApp.Common;

namespace PulseApp.Application.Handlers;

/// <summary>
/// Обработчик подписок
/// </summary>
public class SubscriptionHandler : ISubscriptionHandler
{
    /// <inheritdoc cref="ISubscriptionService"/>
    private readonly ISubscriptionService _service;

    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<SubscriptionHandler> _logger;

    public SubscriptionHandler(ISubscriptionService service,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ILogger<SubscriptionHandler> logger)
    {
        _service = service;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<SubscribeResponse> Subscribe(SubscribeRequest request, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(request.Endpoint) || string.IsNullOrWhiteSpace(request.Keys.P256dh) || string.IsNullOrWhiteSpace(request.Keys.Auth))
        {
            throw new CommonErrorException("Invalid subscription data");
        }

        var subscriptionId = await _service.CreateOrUpdateSubscription(
            request.Endpoint,
            request.Keys.P256dh,
            request.Keys.Auth,
            request.UserAgent,
            request.InviteCode,
            token);

        _logger.LogInformation("New push subscription created: {SubscriptionId}", subscriptionId);

        return new SubscribeResponse(subscriptionId, "Subscription created successfully");
    }

    /// <inheritdoc/>
    public async Task<UnsubscribeResponse> Unsubscribe(string endpoint, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(endpoint))
        {
            throw new CommonErrorException("Invalid endpoint");
        }

        bool success = await _service.DeactivateSubscriptionByEndpoint(endpoint, token);

        if (!success)
        {
            _logger.LogWarning("Subscription not found for endpoint: {Endpoint}", endpoint);
            return new UnsubscribeResponse(false, "Subscription not found");
        }

        _logger.LogInformation("Subscription deactivated for endpoint: {Endpoint}", endpoint);
        return new UnsubscribeResponse(true, "Successfully unsubscribed");
    }

    public async Task<string> CreateSubscriptionCode(CancellationToken token)
    {
        var currentUserId = _currentUserService.GetCurrentUserId();

        return await _service.CreateSubscriptionCode(currentUserId, token);
    }

    public async Task<List<SubscriptionDto>> GetAllSubscriptionForUser(CancellationToken token)
    {
        var sub = await _service.GetAllSubscriptionForUser(token);
        return _mapper.Map<List<SubscriptionDto>>(sub);
    }
}
