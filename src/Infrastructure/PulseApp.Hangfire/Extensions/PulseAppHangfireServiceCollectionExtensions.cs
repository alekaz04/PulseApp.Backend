using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

        services.Configure<HangfireOptions>(configuration.GetSection(nameof(HangfireOptions)));
        return services;
    }

    public static IApplicationBuilder UseCustomHangfire(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetRequiredService<IOptions<HangfireOptions>>().Value;

        app.UseHangfireDashboard(options.Url, new DashboardOptions
        {
            Authorization = new IDashboardAuthorizationFilter[]
            {
                new BasicAuthAuthorizationFilter(options.User, options.Password)
            }
        });

        return app;
    }

    public static IServiceCollection AddHangfireJob<TJob>(this IServiceCollection services)
        where TJob : class, IHangfireRecurringJob
    {
        services.AddSingleton<IHangfireRecurringJob, TJob>();

        return services;
    }
}
