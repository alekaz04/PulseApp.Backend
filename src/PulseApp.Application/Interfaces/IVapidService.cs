// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using PulseApp.Application.Models;

namespace PulseApp.Application.Interfaces;

/// <summary>
/// Сервис для работы с VAPID ключами
/// </summary>
public interface IVapidService
{
    /// <summary>
    /// Получить или сгенерировать VAPID ключи
    /// </summary>
    Task<VapidKeys> GetOrGenerateVapidKeysAsync();

    /// <summary>
    /// Получить публичный ключ
    /// </summary>
    Task<string> GetPublicKeyAsync();
}
