using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using PulseApp.Domain.Options;
using PulseApp.Infrastructure;

namespace PulseApp.Common.Extensions;

/// <summary>
/// Класс расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class CommonServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Добавить основные сервисы
        /// </summary>
        public IServiceCollection AddCommon(IConfiguration configuration)
        {
            services.AddSwagger();
            services.AddHealthChecks()
                .AddCheck(
                    name: "liveness",
                    check: () => HealthCheckResult.Healthy(),
                    tags: new string[] { "liveness" })
                .AddDbContextCheck<PulseDataContext>();

            return services;
        }

        /// <summary>
        /// Добавить сваггер
        /// </summary>
        private IServiceCollection AddSwagger()
        {
            services.AddOpenApiDocument((config, serviceProvider) =>
            {
                var keycloak = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
                string authority = keycloak.Authority.TrimEnd('/');

                config.Title = "PulseApp API";

                config.AddSecurity("oauth2", new NSwag.OpenApiSecurityScheme
                {
                    Type = NSwag.OpenApiSecuritySchemeType.OAuth2,
                    Flow = NSwag.OpenApiOAuth2Flow.AccessCode,
                    AuthorizationUrl = $"{authority}/protocol/openid-connect/auth",
                    TokenUrl = $"{authority}/protocol/openid-connect/token",
                    Scopes = new Dictionary<string, string>
                    {
                        { "openid", "OpenID" },
                        { "profile", "Profile" }
                    }
                });

                config.OperationProcessors.Add(
                    new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("oauth2"));
            });
            return services;
        }
    }
}
