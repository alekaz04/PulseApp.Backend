// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PulseApp.Domain.Entities;

/// <summary>
/// Код подписки к уведомлениям
/// </summary>
public class SubscriptionCode
{
    /// <summary>
    /// Идентификатор кода
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Код
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// Дата и время создания кода
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Дата и время "протухания" кода
    /// </summary>
    public DateTimeOffset ExpireAt { get; set; }

    /// <summary>
    /// Идентификатор пользователя создавшего код
    /// </summary>
    public Guid CreatedCodeUserId { get; set; }

    /// <summary>
    /// Флаг использованности кода
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// Пользователя создавшего код
    /// </summary>
    public User CreatedCodeUser { get; set; } = null!;
}
