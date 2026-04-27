// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PulseApp.Domain.Options;

/// <summary>
/// Опции API ключа
/// </summary>
public class ApiKeyOption
{
    /// <summary>
    /// Ключ
    /// </summary>
    public string Key { get; set; } = null!;
}
