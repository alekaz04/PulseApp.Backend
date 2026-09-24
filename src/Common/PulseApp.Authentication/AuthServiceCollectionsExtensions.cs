// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PulseApp.Authentication;
using PulseApp.Authentication.Abstraction;
using PulseApp.Authentication.Middlewars;
using PulseApp.Authentication.Services;
using PulseApp.Domain.Options;

namespace PulseApp.Extensions.DependencyInjection;

public static class AuthServiceCollectionsExtensions
{
    /// <summary>
    /// Добавить аутентификацию через Keycloak (JWT Bearer), политики авторизации и сервисы пользователя.
    /// Настройки берутся из <see cref="KeycloakOptions"/>
    /// </summary>
    public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<KeycloakOptions>>((options, keycloakOptions) =>
                ConfigureJwtBearer(options, keycloakOptions.Value));

        services.AddAuthorization();
        services.AddOptions<AuthorizationOptions>()
            .Configure<IOptions<KeycloakOptions>>((options, keycloakOptions) =>
                options.AddPolicy(AuthorizationPolicies.Admin, policy => policy
                    .RequireAuthenticatedUser()
                    .RequireRole(keycloakOptions.Value.AdminRole)));

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    /// <summary>
    /// Определять пользователя запроса и создавать его в БД при первом входе.
    /// Вызывать после UseAuthentication и UseAuthorization
    /// </summary>
    public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app)
    {
        app.UseMiddleware<CurrentUserMiddleware>();
        return app;
    }

    /// <summary>
    /// Настроить проверку токенов Keycloak
    /// </summary>
    private static void ConfigureJwtBearer(JwtBearerOptions options, KeycloakOptions keycloak)
    {
        options.Authority = keycloak.Authority;
        options.Audience = keycloak.Audience;
        options.RequireHttpsMetadata = keycloak.RequireHttpsMetadata;
        options.MapInboundClaims = false;

        if (!string.IsNullOrWhiteSpace(keycloak.MetadataAddress))
        {
            options.MetadataAddress = keycloak.MetadataAddress;
        }

        var validation = options.TokenValidationParameters;
        validation.NameClaimType = keycloak.NameClaimType;
        validation.RoleClaimType = KeycloakClaimTypes.Role;
        validation.ClockSkew = keycloak.ClockSkew;

        if (keycloak.ValidIssuers.Length > 0)
        {
            validation.ValidIssuers = keycloak.ValidIssuers;
        }

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                if (context.Principal?.Identity is not ClaimsIdentity identity)
                {
                    context.Fail("Token has no identity");
                    return Task.CompletedTask;
                }

                // С Keycloak 25+ sub в access token есть только при подключённом client scope "basic"
                if (!identity.HasClaim(c => c.Type == KeycloakClaimTypes.Subject))
                {
                    context.Fail($"Token has no '{KeycloakClaimTypes.Subject}' claim");
                    return Task.CompletedTask;
                }

                KeycloakRolesMapper.MapRoles(identity, keycloak.Audience);
                return Task.CompletedTask;
            }
        };
    }
}
