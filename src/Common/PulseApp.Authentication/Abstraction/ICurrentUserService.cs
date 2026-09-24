// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using PulseApp.Domain.Entities;

namespace PulseApp.Authentication.Abstraction;

/// <summary>
/// Пользователь, который выполняет текущий запрос
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Текущий пользователь. null для анонимных запросов
    /// </summary>
    User? CurrentUser { get; set; }
}
