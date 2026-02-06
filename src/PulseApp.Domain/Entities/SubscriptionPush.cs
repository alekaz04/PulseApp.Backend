// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PulseApp.Domain.Entities;

/// <summary>
/// Сущность подписки на push-уведомления
/// </summary>
public class SubscriptionPush
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Endpoint для отправки push-уведомлений (уникальный)
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Публичный ключ клиента для шифрования (P256DH)
    /// </summary>
    public string P256dh { get; set; } = string.Empty;

    /// <summary>
    /// Auth secret для шифрования
    /// </summary>
    public string Auth { get; set; } = string.Empty;

    /// <summary>
    /// Дата создания подписки
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User Agent браузера (опционально)
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// Флаг активности подписки
    /// </summary>
    public bool IsActive { get; set; } = true;
}
