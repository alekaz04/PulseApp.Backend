// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PulseApp.Application.Models;

/// <summary>
/// Модель VAPID ключей для Web Push
/// </summary>
public class VapidKeys
{
    /// <summary>
    /// Публичный ключ (отдается клиенту)
    /// </summary>
    public string PublicKey { get; set; } = string.Empty;

    /// <summary>
    /// Приватный ключ (хранится на сервере)
    /// </summary>
    public string PrivateKey { get; set; } = string.Empty;
}
