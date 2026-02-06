using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;
using PulseApp.Domain.Entities;
using WebPush;

namespace PulseApp.Application.Services;

/// <summary>
/// Реализация сервиса для отправки push-уведомлений
/// </summary>
public class PushNotificationService : IPushNotificationService
{
    /// <inheritdoc cref="IVapidService"/>
    private readonly IVapidService _vapidService;

    /// <inheritdoc cref="ISubscriptionService"/>
    private readonly ISubscriptionService _subscriptionService;

    /// <inheritdoc cref="IConfiguration"/>
    private readonly IConfiguration _configuration;

    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<PushNotificationService> _logger;

    public PushNotificationService(IVapidService vapidService, ISubscriptionService subscriptionService, IConfiguration configuration, ILogger<PushNotificationService> logger)
    {
        _vapidService = vapidService;
        _subscriptionService = subscriptionService;
        _configuration = configuration;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task SendNotificationToSubscriber(SubscriptionPush subscription, PushNotificationPayload payload, CancellationToken token)
    {
        try
        {
            var vapidKeys = _vapidService.GetVapidKeys();

            string subject = _configuration["VapidSettings:Subject"] ?? "mailto:admin@pulseapp.com";

            var vapidDetails = new VapidDetails(
                subject: subject,
                publicKey: vapidKeys.PublicKey,
                privateKey: vapidKeys.PrivateKey
            );

            var pushSubscription = new PushSubscription(
                endpoint: subscription.Endpoint,
                p256dh: subscription.P256dh,
                auth: subscription.Auth
            );

            var webPushClient = new WebPushClient();

            object jsonPayload = new
            {
                notification = new
                {
                    title = payload.Title,
                    body = payload.Body,
                    icon = payload.Icon,
                    badge = payload.Badge,
                    data = payload.Data ?? new Dictionary<string, object> { ["url"] = "/" }
                }
            };

            string payloadString = JsonSerializer.Serialize(jsonPayload);

            await webPushClient.SendNotificationAsync(pushSubscription, payloadString, vapidDetails, token);

            _logger.LogInformation("Push notification sent successfully to {Endpoint}", subscription.Endpoint[..50] + "...");
        }
        catch (WebPushException ex)
        {
            _logger.LogWarning(ex, "Failed to send push notification. Status: {StatusCode}", ex.StatusCode);

            if (ex.StatusCode is HttpStatusCode.Gone or HttpStatusCode.NotFound)
            {
                _logger.LogInformation("Deactivating subscription {SubscriptionId} due to {StatusCode}", subscription.Id, ex.StatusCode);

                await _subscriptionService.DeactivateSubscription(subscription.Id, token);
            }
        }
    }

    /// <inheritdoc/>
    public async Task SendNotificationToAllAsync(PushNotificationPayload payload, CancellationToken token)
    {
        var subscriptions = await _subscriptionService.GetActiveSubscriptions(token);

        if (subscriptions.Count == 0)
        {
            _logger.LogWarning("No active subscriptions found. Skipping push notification.");
            return;
        }
        _logger.LogInformation("Sending push notification to {Count} subscriptions", subscriptions.Count);
        var tasks = subscriptions.Select(sub => SendNotificationToSubscriber(sub, payload, token))
            .ToList();

        await Task.WhenAll(tasks);

        _logger.LogInformation("Push notifications sent to {Count} subscriptions", subscriptions.Count);
    }

    /// <inheritdoc/>
    public async Task SendNotificationToSubscribers(List<SubscriptionPush> subscriptions, PushNotificationPayload payload, CancellationToken token)
    {
        var tasks = subscriptions.Select(sub => SendNotificationToSubscriber(sub, payload, token)).ToList();

        await Task.WhenAll(tasks);

        _logger.LogInformation("Push notifications sent to {Count} subscriptions", subscriptions.Count
        );
    }
}
