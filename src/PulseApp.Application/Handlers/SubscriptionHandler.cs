using Microsoft.Extensions.Logging;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;
using PulseApp.Common;

namespace PulseApp.Application.Handlers;

/// <summary>
/// Обработчик подписок
/// </summary>
public class SubscriptionHandler : ISubscriptionHandler
{
    /// <inheritdoc cref="ISubscriptionService"/>
    private readonly ISubscriptionService _service;

    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<SubscriptionHandler> _logger;

    public SubscriptionHandler(ISubscriptionService service, ILogger<SubscriptionHandler> logger)
    {
        _service = service;
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
}
