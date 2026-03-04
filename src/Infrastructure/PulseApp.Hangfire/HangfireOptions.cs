// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PulseApp.Hangfire;

/// <summary>
/// Опции Hangfire
/// </summary>
public class HangfireOptions
{
    /// <summary>
    /// Логин пользователя hangfire
    /// </summary>
    public string User { get; set; } = "hangfire";

    /// <summary>
    /// Пароль пользователя hangfire
    /// </summary>
    public string Password { get; set; } = "hangfire";

    /// <summary>
    /// Url дашборда hangfire
    /// </summary>
    public string Url {get;set;} = "/hangfire";
}
