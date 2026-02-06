using Microsoft.AspNetCore.Builder;

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
            app.UseMiddleware<ErrorMiddleware.ErrorMiddleware>();
            return app;
        }
    }
}
