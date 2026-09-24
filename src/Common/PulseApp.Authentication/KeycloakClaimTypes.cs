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

    /// <summary>
    /// Роли realm'а: {"roles": [...]}
    /// </summary>
    public const string RealmAccess = "realm_access";

    /// <summary>
    /// Роли клиентов: {"client-id": {"roles": [...]}}
    /// </summary>
    public const string ResourceAccess = "resource_access";

    /// <summary>
    /// Плоский клейм роли, в который переносятся роли из <see cref="RealmAccess"/> и <see cref="ResourceAccess"/>
    /// </summary>
    public const string Role = "role";
}
