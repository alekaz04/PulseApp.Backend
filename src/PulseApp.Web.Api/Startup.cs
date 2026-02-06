// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using PulseApp.Application;
using PulseApp.Common.Extensions;
using PulseApp.Domain.Options;
using PulseApp.Infrastructure;
using PulseApp.Logging.Extensions;

namespace PulseApp.Web.Api;

/// <summary>
/// Конфигурация сервера
/// </summary>
public class Startup
{
    /// <inheritdoc cref="IConfiguration"/>
    private IConfiguration Configuration { get; }

    private const string CorsPolicy = "PWAPolicy";

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
            .AddPostgresLogging(Configuration)
            .AddApplicationServices();

        services.AddDbContext<PulseDataContext>(x =>
            x.UseNpgsql(Configuration.GetConnectionString(nameof(PulseDataContext))));

        // Hangfire
        string connectionString = Configuration.GetConnectionString(nameof(PulseDataContext))
                                 ?? throw new InvalidOperationException("Connection string 'PulseDataContext' not found");

        services.Configure<VapidOptions>(Configuration.GetSection(nameof(VapidOptions)));

        services.AddHangfire(config =>
        {
            config.UsePostgreSqlStorage(options =>
                {
                    options.UseNpgsqlConnection(connectionString);
                });
        });

        services.AddHangfireServer();

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy, policy =>
            {
                policy.WithOrigins(Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [""])
                    .SetIsOriginAllowed(origin => origin.StartsWith("http://localhost") || origin.StartsWith("http://127.0.0.1"))
                    .AllowAnyMethod()
                    .AllowAnyHeader();
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

        app.UseCors(CorsPolicy);

        app.UseRouting();

        app.UseHangfireDashboard("/admin/hangfire");

        app.UseEndpoints(x =>
        {
            x.MapControllers();
            x.MapHangfireDashboard();
        });
    }
}
