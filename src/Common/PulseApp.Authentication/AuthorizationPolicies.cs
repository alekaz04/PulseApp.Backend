// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PulseApp.Authentication;

/// <summary>
/// Политики авторизации
/// </summary>
public static class AuthorizationPolicies
{
    /// <summary>
    /// Доступ только для администратора системы
    /// </summary>
    public const string Admin = nameof(Admin);
}
