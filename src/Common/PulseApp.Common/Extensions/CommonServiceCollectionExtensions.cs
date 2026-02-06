using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
