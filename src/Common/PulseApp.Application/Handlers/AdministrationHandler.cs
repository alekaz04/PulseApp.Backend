using Microsoft.Extensions.Logging;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;
using PulseApp.Common;
using PulseApp.Domain.Entities;

namespace PulseApp.Application.Handlers;

/// <summary>
/// Администрирование системы
/// </summary>
public class AdministrationHandler : IAdministrationHandler
{
    /// <inheritdoc cref="ISubscriptionService"/>
    private readonly ISubscriptionService _service;

    /// <inheritdoc cref="IPushNotificationService"/>
    private readonly IPushNotificationService _pushNotificationService;

    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<AdministrationHandler> _logger;

    public AdministrationHandler(ISubscriptionService service, IPushNotificationService pushNotificationService, ILogger<AdministrationHandler> logger)
    {
        _service = service;
        _pushNotificationService = pushNotificationService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<SendNotificationResponse> SendToAll(SendNotificationRequest request, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new CommonErrorException("Title is required");
        }

        if (string.IsNullOrWhiteSpace(request.Body))
        {
            throw new CommonErrorException("Body is required");
        }

        var subscriptions = await _service.GetActiveSubscriptions(token);
        int totalSubscriptions = subscriptions.Count;

        if (totalSubscriptions == 0)
        {
            _logger.LogWarning("No active subscriptions found");
            return new SendNotificationResponse(0, "No active subscriptions");
        }

        var payload = new PushNotificationPayload(
            Title: request.Title,
            Body: request.Body,
            Icon: request.Icon ?? "/favicon/web-app-manifest-192x192.png",
            Badge: request.Badge ?? "/favicon/favicon-96x96.png"
        );

        // Отправляем всем подписчикам
        await _pushNotificationService.SendNotificationToSubscribers(subscriptions, payload, token);

        _logger.LogInformation(
            "Push notification sent to {Count} subscriptions: {Title}",
            totalSubscriptions,
            request.Title
        );

        return new SendNotificationResponse(totalSubscriptions, $"Notification sent to {totalSubscriptions} subscriber(s)");
    }

    /// <inheritdoc/>
    public async Task<List<SubscriptionPush>> GetAllSubscriptions(CancellationToken token)
    {
        return await _service.GetActiveSubscriptions(token);
    }
}
