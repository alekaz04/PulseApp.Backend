using Microsoft.Extensions.DependencyInjection;
using PulseApp.Application.Handlers;
using PulseApp.Application.Interfaces;
using PulseApp.Application.Jobs;
using PulseApp.Application.Services;
using PulseApp.Extensions.DependencyInjection;

namespace PulseApp.Application;

/// <summary>
/// Класс расширений для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить сервисы приложения
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IVapidService, VapidService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IPushNotificationService, PushNotificationService>();

        services.AddScoped<IAdministrationHandler, AdministrationHandler>();
        services.AddScoped<ISubscriptionHandler, SubscriptionHandler>();

        services.AddHangfireJob<GiveComplimentJob>();

        return services;
    }
}
