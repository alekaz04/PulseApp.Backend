// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PulseApp.Domain.Entities;

/// <summary>
/// Сущность комплимента
/// </summary>
public class Compliments
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Текст
    /// </summary>
    public string Text { get; set; } = null!;

    /// <summary>
    /// Категория
    /// </summary>
    public string? Category {get; set;}

    /// <summary>
    /// Флаг активации
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}
