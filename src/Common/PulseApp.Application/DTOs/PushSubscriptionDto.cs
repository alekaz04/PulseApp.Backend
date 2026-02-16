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
    string? Icon = "/favicon/favicon-96x96.png",
    string? Badge = "/favicon/web-app-manifest-192x192.png",
    Dictionary<string, object>? Data = null
);

/// <summary>
/// Запрос на отправку уведомления всем подписчикам
/// </summary>
public record SendNotificationRequest(
    string Title,
    string Body,
    string? Icon = null,
    string? Badge = null
);

/// <summary>
/// Ответ на запрос отправки уведомления
/// </summary>
public record SendNotificationResponse(
    int TotalSubscriptions,
    string Message
);

/// <summary>
/// Ответ на запрос отписки от push-уведомлений
/// </summary>
public record UnsubscribeResponse(
    bool Success,
    string Message
);
