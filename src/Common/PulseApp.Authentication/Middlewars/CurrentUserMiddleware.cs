using Microsoft.AspNetCore.Http;
using PulseApp.Authentication.Abstraction;

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

    public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUser, IUserService userService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var current = await userService.GetOrCreateAsync(context.User, context.RequestAborted);
            currentUser.SetCurrentUser(current);
        }

        await _next(context);
    }
}
