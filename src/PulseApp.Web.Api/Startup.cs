// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using PulseApp.Application.Interfaces;
using PulseApp.Extensions.DependencyInjection;
using PulseApp.Infrastructure;
using PulseApp.Infrastructure.Services;

namespace PulseApp.Web.Api;

/// <summary>
/// Конфигурация сервера
/// </summary>
public class Startup
{
    /// <inheritdoc cref="IConfiguration"/>
    private IConfiguration Configuration { get; }

    /// <summary>
    /// Инициализация приложения
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Инициализация сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddCommon(Configuration)
            .AddPostgresLogging(Configuration);

        // Database
        services.AddDbContext<PulseDataContext>(x => 
            x.UseNpgsql(Configuration.GetConnectionString(nameof(PulseDataContext))));

        // Push Notification Services
        services.AddScoped<IVapidService, VapidService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IPushNotificationService, PushNotificationService>();

        // Hangfire
        string connectionString = Configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

        services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(options =>
                {
                    options.UseNpgsqlConnection(connectionString);
                });
        });

        services.AddHangfireServer();

        // CORS для PWA
        services.AddCors(options =>
        {
            options.AddPolicy("PWAPolicy", policy =>
            {
                policy.WithOrigins(
                        "https://localhost:4200",
                        "http://localhost:4200"
                    )
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }

    /// <summary>
    /// Конфигурация сервисов
    /// </summary>
    /// <param name="app"><inheritdoc cref="IApplicationBuilder"/></param>
    /// <param name="env"><inheritdoc cref="IWebHostEnvironment"/></param>
    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();

        app.UseErrorMiddleware();

        // CORS должен быть до UseRouting
        app.UseCors("PWAPolicy");

        app.UseRouting();

        // Hangfire Dashboard
        app.UseHangfireDashboard("/admin/hangfire");

        app.UseEndpoints(x => 
        {
            x.MapControllers();
            x.MapHangfireDashboard();
        });
    }
}
