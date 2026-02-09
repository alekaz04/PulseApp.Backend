using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PulseApp.Hangfire;
using PulseApp.Infrastructure;

namespace PulseApp.Extensions.DependencyInjection;

public static class PulseAppHangfireServiceCollectionExtensions
{
    public static IServiceCollection AddCustomHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(x =>
            x.UsePostgreSqlStorage(options =>
                options.UseNpgsqlConnection(configuration.GetConnectionString(nameof(PulseDataContext)))))
            .AddHangfireServer()
            .AddHostedService<HangfireBackgroundService>();

        return services;
    }

    public static IApplicationBuilder UseCustomHangfire(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard();
        return app;
    }

    public static IServiceCollection AddHangfireJob<TJob>(this IServiceCollection services)
        where TJob : class, IHangfireRecurringJob
    {
        services.AddSingleton<IHangfireRecurringJob, TJob>();

        return services;
    }
}
