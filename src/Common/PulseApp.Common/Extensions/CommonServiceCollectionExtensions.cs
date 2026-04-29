using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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
            services.AddOpenApiDocument();
            return services;
        }
    }
}
