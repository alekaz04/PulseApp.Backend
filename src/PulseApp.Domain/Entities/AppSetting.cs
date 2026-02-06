// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PulseApp.Domain.Entities;

/// <summary>
/// Сущность настроек приложения
/// </summary>
public class AppSetting
{
    /// <summary>
    /// Ключ настройки (первичный ключ)
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Значение настройки (может быть JSON для сложных объектов)
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}
