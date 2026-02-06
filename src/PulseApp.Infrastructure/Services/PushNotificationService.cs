// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;
using PulseApp.Application.Models;
using WebPush;
using DomainPushSubscription = PulseApp.Domain.Entities.PushSubscription;

namespace PulseApp.Infrastructure.Services;

/// <summary>
/// Реализация сервиса для отправки push-уведомлений
/// </summary>
public class PushNotificationService : IPushNotificationService
{
    private readonly IVapidService _vapidService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PushNotificationService> _logger;

    public PushNotificationService(
        IVapidService vapidService,
        ISubscriptionService subscriptionService,
        IConfiguration configuration,
        ILogger<PushNotificationService> logger)
    {
        _vapidService = vapidService;
        _subscriptionService = subscriptionService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendNotificationAsync(
        DomainPushSubscription subscription,
        PushNotificationPayload payload)
    {
        try
        {
            VapidKeys vapidKeys = await _vapidService.GetOrGenerateVapidKeysAsync();

            string subject = _configuration["VapidSettings:Subject"] ?? "mailto:admin@pulseapp.com";

            VapidDetails vapidDetails = new VapidDetails(
                subject: subject,
                publicKey: vapidKeys.PublicKey,
                privateKey: vapidKeys.PrivateKey
            );

            WebPush.PushSubscription pushSubscription = new WebPush.PushSubscription(
                endpoint: subscription.Endpoint,
                p256dh: subscription.P256dh,
                auth: subscription.Auth
            );

            WebPushClient webPushClient = new WebPushClient();

            // Формируем payload в формате, ожидаемом Service Worker
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

            await webPushClient.SendNotificationAsync(
                pushSubscription,
                payloadString,
                vapidDetails
            );

            _logger.LogInformation(
                "Push notification sent successfully to {Endpoint}",
                subscription.Endpoint[..50] + "..."
            );
        }
        catch (WebPushException ex)
        {
            _logger.LogWarning(
                ex,
                "Failed to send push notification. Status: {StatusCode}",
                ex.StatusCode
            );

            // Если подписка устарела или недействительна - деактивируем её
            if (ex.StatusCode == HttpStatusCode.Gone ||
                ex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogInformation(
                    "Deactivating subscription {SubscriptionId} due to {StatusCode}",
                    subscription.Id,
                    ex.StatusCode
                );

                await _subscriptionService.DeactivateSubscriptionAsync(subscription.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error sending push notification");
            throw;
        }
    }

    public async Task SendNotificationToAllAsync(PushNotificationPayload payload)
    {
        List<DomainPushSubscription> subscriptions = await _subscriptionService.GetActiveSubscriptionsAsync();

        if (subscriptions.Count == 0)
        {
            _logger.LogWarning("No active subscriptions found. Skipping push notification.");
            return;
        }

        _logger.LogInformation(
            "Sending push notification to {Count} subscriptions",
            subscriptions.Count
        );

        List<Task> tasks = subscriptions.Select(sub =>
            SendNotificationAsync(sub, payload)
        ).ToList();

        await Task.WhenAll(tasks);

        _logger.LogInformation(
            "Push notifications sent to {Count} subscriptions",
            subscriptions.Count
        );
    }
}
