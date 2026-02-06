using Microsoft.AspNetCore.Builder;
using PulseApp.Common;

namespace PulseApp.Extensions.DependencyInjection;

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
            app.UseOpenApi();
            app.UseSwaggerUi(options =>
            {
                options.Path = string.Empty;
                options.DocumentTitle = "PulseApp API";
            });
            return app;
        }

        /// <summary>
        /// Добавить миддлвар отлова ошибок
        /// </summary>
        public IApplicationBuilder UseErrorMiddleware()
        {
            app.UseMiddleware<ErrorMiddleware>();
            return app;
        }
    }
}
