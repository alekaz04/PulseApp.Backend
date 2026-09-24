using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PulseApp.Domain.Options;

namespace PulseApp.Common.Extensions;

/// <summary>
/// Класс расширений для <see cref="IApplicationBuilder"/>
/// </summary>
public static class CommonApplicationBuilderExtensions
{
    extension(IApplicationBuilder app)
    {
        /// <summary>
        /// Добавить сваггер
        /// </summary>
        public IApplicationBuilder UseSwagger()
        {
            var keycloak = app.ApplicationServices.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            app.UseOpenApi();
            app.UseSwaggerUi(options =>
            {
                options.Path = "/swagger";
                options.DocumentTitle = "PulseApp API";

                options.OAuth2Client = new NSwag.AspNetCore.OAuth2ClientSettings
                {
                    ClientId = keycloak.SwaggerClientId,
                    UsePkceWithAuthorizationCodeGrant = true,
                    AdditionalQueryStringParameters = { { "scope", "openid profile" } }
                };
            });

            return app;
        }

        /// <summary>
        /// Добавить миддлвар отлова ошибок
        /// </summary>
        public IApplicationBuilder UseErrorMiddleware()
        {
            app.UseMiddleware<ErrorMiddleware.ErrorMiddleware>();
            return app;
        }
    }
}
