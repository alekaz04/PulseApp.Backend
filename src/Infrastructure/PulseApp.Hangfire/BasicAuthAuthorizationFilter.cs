// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using Hangfire.Dashboard;

namespace PulseApp.Hangfire;

public class BasicAuthAuthorizationFilter : IDashboardAuthorizationFilter
{
    private readonly string _username;
    private readonly string _password;

    public BasicAuthAuthorizationFilter(string username, string password)
    {
        _username = username;
        _password = password;
    }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        string? authHeader = httpContext.Request.Headers.Authorization;

        if (authHeader != null && authHeader.StartsWith("Basic "))
        {
            string encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            string credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var parts = credentials.Split(':');

            if (parts.Length == 2)
            {
                return parts[0] == _username && parts[1] == _password;
            }
        }

        httpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hangfire Dashboard\"";
        httpContext.Response.StatusCode = 401;
        return false;
    }
}
