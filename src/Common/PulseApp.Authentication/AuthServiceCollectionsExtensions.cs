// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Builder;

namespace PulseApp.Extensions.DependencyInjection;

public static class AuthServiceCollectionsExtensions
{
    public static IApplicationBuilder UseApiProtection(this IApplicationBuilder app)
    {
        //app.UseMiddleware<ApiKeyCheckMiddleware>();
        return app;
    }
}
