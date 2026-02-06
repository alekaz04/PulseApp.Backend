// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using PulseApp.Application.DTOs;
using PulseApp.Domain.Entities;

namespace PulseApp.Application.Interfaces;

/// <summary>
/// Сервис для отправки push-уведомлений
/// </summary>
public interface IPushNotificationService
{
    /// <summary>
    /// Отправить push-уведомление одному подписчику
    /// </summary>
    Task SendNotificationAsync(PushSubscription subscription, PushNotificationPayload payload);

    /// <summary>
    /// Отправить push-уведомление всем подписчикам
    /// </summary>
    Task SendNotificationToAllAsync(PushNotificationPayload payload);
}
