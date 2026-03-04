// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PulseApp.Domain.Options;

/// <summary>
/// Опции бд постгрес
/// </summary>
public class DatabaseOptions
{
    /// <summary>
    /// Флаг выполнения миграций при запуске приложения
    /// </summary>
    public bool Migrate { get; set; }
}
