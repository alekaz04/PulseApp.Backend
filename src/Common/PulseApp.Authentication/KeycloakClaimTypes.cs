// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PulseApp.Authentication;

/// <summary>
/// Типы клеймов токена Keycloak
/// </summary>
public static class KeycloakClaimTypes
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public const string Subject = "sub";

    /// <summary>
    /// Почта пользователя
    /// </summary>
    public const string Email = "email";
}
