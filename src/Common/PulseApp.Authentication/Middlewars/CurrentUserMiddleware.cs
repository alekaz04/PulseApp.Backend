// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http;
using PulseApp.Authentication.Abstraction;
using PulseApp.Authentication.Services;

namespace PulseApp.Authentication.Middlewars;

/// <summary>
/// Миддлвар определения пользователя
/// </summary>
public class CurrentUserMiddleware
{
    /// <inheritdoc cref="RequestDelegate"/>
    private readonly RequestDelegate _next;

    public CurrentUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, CurrentUserService currentUser, IUserService userService)
    {
        // Пропускаем анонимные запросы (например, /swagger) — middleware не должен их ломать
        if (context.User.Identity?.IsAuthenticated == true)
        {
            currentUser.CurrentUser = await userService.GetOrCreateAsync(context.User, context.RequestAborted);
        }

        await _next(context);
    }
}
