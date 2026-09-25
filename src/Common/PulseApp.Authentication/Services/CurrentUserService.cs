// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using PulseApp.Authentication.Abstraction;
using PulseApp.Domain.Entities;

namespace PulseApp.Authentication.Services;

/// <inheritdoc/>
public class CurrentUserService : ICurrentUserService
{
    /// <inheritdoc/>
    public User? CurrentUser { get; set; }
}
