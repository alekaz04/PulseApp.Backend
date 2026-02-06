// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PulseApp.Application.DTOs;

/// <summary>
/// DTO для создания подписки на push-уведомления
/// </summary>
public record SubscribeRequest(
    string Endpoint,
    PushKeys Keys,
    string? UserAgent
);

/// <summary>
/// Ключи шифрования для push-подписки
/// </summary>
public record PushKeys(
    string P256dh,
    string Auth
);

/// <summary>
/// Ответ на запрос создания подписки
/// </summary>
public record SubscribeResponse(
    Guid Id,
    string Message
);

/// <summary>
/// Ответ с VAPID публичным ключом
/// </summary>
public record VapidPublicKeyResponse(
    string PublicKey
);

/// <summary>
/// Модель payload для push-уведомления
/// </summary>
public record PushNotificationPayload(
    string Title,
    string Body,
    string? Icon = "/icons/icon-192x192.png",
    string? Badge = "/icons/badge-72x72.png",
    Dictionary<string, object>? Data = null
);
