// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;

namespace PulseApp.Web.Api.Controllers;

/// <summary>
/// API для работы с push-уведомлениями
/// </summary>
[ApiController]
[Route("api/push")]
public class PushController : ControllerBase
{
    private readonly IVapidService _vapidService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IPushNotificationService _pushNotificationService;
    private readonly ILogger<PushController> _logger;

    public PushController(
        IVapidService vapidService,
        ISubscriptionService subscriptionService,
        IPushNotificationService pushNotificationService,
        ILogger<PushController> logger)
    {
        _vapidService = vapidService;
        _subscriptionService = subscriptionService;
        _pushNotificationService = pushNotificationService;
        _logger = logger;
    }

    /// <summary>
    /// Получить VAPID публичный ключ (КРИТИЧНО для PWA)
    /// </summary>
    [HttpGet("vapid-public-key")]
    [ProducesResponseType(typeof(VapidPublicKeyResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<VapidPublicKeyResponse>> GetVapidPublicKey()
    {
        try
        {
            string publicKey = await _vapidService.GetPublicKeyAsync();
            return Ok(new VapidPublicKeyResponse(publicKey));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get VAPID public key");
            return StatusCode(500, "Failed to get public key");
        }
    }

    /// <summary>
    /// Подписаться на push-уведомления (КРИТИЧНО для PWA)
    /// </summary>
    [HttpPost("subscribe")]
    [ProducesResponseType(typeof(SubscribeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SubscribeResponse>> Subscribe([FromBody] SubscribeRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Endpoint) ||
                string.IsNullOrWhiteSpace(request.Keys.P256dh) ||
                string.IsNullOrWhiteSpace(request.Keys.Auth))
            {
                return BadRequest("Invalid subscription data");
            }

            Guid subscriptionId = await _subscriptionService.CreateOrUpdateSubscriptionAsync(
                request.Endpoint,
                request.Keys.P256dh,
                request.Keys.Auth,
                request.UserAgent
            );

            _logger.LogInformation("New push subscription created: {SubscriptionId}", subscriptionId);

            return Ok(new SubscribeResponse(
                subscriptionId,
                "Subscription created successfully"
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create push subscription");
            return StatusCode(500, "Failed to create subscription");
        }
    }

    /// <summary>
    /// Отписаться от push-уведомлений
    /// </summary>
    [HttpDelete("unsubscribe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Unsubscribe([FromQuery] string endpoint)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                return BadRequest("Endpoint is required");
            }

            await _subscriptionService.DeleteSubscriptionByEndpointAsync(endpoint);

            _logger.LogInformation("Push subscription deleted for endpoint: {Endpoint}", endpoint[..50] + "...");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete push subscription");
            return StatusCode(500, "Failed to delete subscription");
        }
    }

    /// <summary>
    /// Отправить push-уведомление всем подписчикам (без авторизации для тестирования)
    /// </summary>
    [HttpPost("send-to-all")]
    [ProducesResponseType(typeof(SendNotificationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SendNotificationResponse>> SendToAll([FromBody] SendNotificationRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest("Title is required");
            }

            if (string.IsNullOrWhiteSpace(request.Body))
            {
                return BadRequest("Body is required");
            }

            // Получаем активные подписки для статистики
            var subscriptions = await _subscriptionService.GetActiveSubscriptionsAsync();
            int totalSubscriptions = subscriptions.Count;

            if (totalSubscriptions == 0)
            {
                _logger.LogWarning("No active subscriptions found");
                return Ok(new SendNotificationResponse(0, "No active subscriptions"));
            }

            // Формируем payload
            var payload = new PushNotificationPayload(
                Title: request.Title,
                Body: request.Body,
                Icon: request.Icon ?? "/icons/icon-192x192.png",
                Badge: request.Badge ?? "/icons/badge-72x72.png"
            );

            // Отправляем всем подписчикам (параллельно)
            await _pushNotificationService.SendNotificationToAllAsync(payload);

            _logger.LogInformation(
                "Push notification sent to {Count} subscriptions: {Title}",
                totalSubscriptions,
                request.Title
            );

            return Ok(new SendNotificationResponse(
                totalSubscriptions,
                $"Notification sent to {totalSubscriptions} subscriber(s)"
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send push notification to all subscribers");
            return StatusCode(500, "Failed to send notifications");
        }
    }
}
