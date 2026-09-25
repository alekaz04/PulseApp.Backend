using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;
using PulseApp.Domain.Entities;
using PulseApp.Domain.Options;
using WebPush;

namespace PulseApp.Application.Services;

/// <summary>
/// Реализация сервиса для отправки push-уведомлений
/// </summary>
public class PushNotificationService : IPushNotificationService
{
    /// <inheritdoc cref="IOptions{T}"/>
    private readonly IOptions<VapidOptions> _vapidOptions;

    /// <inheritdoc cref="ISubscriptionService"/>
    private readonly ISubscriptionService _subscriptionService;

    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<PushNotificationService> _logger;

    public PushNotificationService(IOptions<VapidOptions> vapidOptions, ISubscriptionService subscriptionService, ILogger<PushNotificationService> logger)
    {
        _vapidOptions = vapidOptions;
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    public async Task SendComplimentNotification(Subscription subscription, Compliment compliment, CancellationToken token)
    {
        using var webPushClient = new WebPushClient();

        var payload = new PushNotificationPayload(
            Title: compliment.Title,
            Body: compliment.Text,
            Icon: "/favicon/web-app-manifest-192x192.png",
            Badge: "/favicon/favicon-96x96.png"
        );
        await SendNotificationToSubscriber(subscription, webPushClient, payload, token);
    }


    private async Task SendNotificationToSubscriber(Subscription subscription, WebPushClient webPushClient, PushNotificationPayload payload, CancellationToken token)
    {
        try
        {
            var vapidOptions = _vapidOptions.Value;

            var vapidDetails = new VapidDetails(
                subject: vapidOptions.Subject,
                publicKey: vapidOptions.PublicKey,
                privateKey: vapidOptions.PrivateKey
            );

            var pushSubscription = new PushSubscription(
                endpoint: subscription.Endpoint,
                p256dh: subscription.P256dh,
                auth: subscription.Auth
            );

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
                throw;
            }
        }
    }
}
