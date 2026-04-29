using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using PulseApp.Application;
using PulseApp.Common.Extensions;
using PulseApp.Extensions.DependencyInjection;
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
            .AddApplicationServices(Configuration)
            .AddPulseOptions(Configuration)
            .AddCustomHangfire(Configuration);

        services.AddDbContext<PulseDataContext>(x =>
            x.UseNpgsql(Configuration.GetConnectionString(nameof(PulseDataContext))));

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy, policy =>
            {
                policy.WithOrigins(Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [""])
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
        app.UseApiProtection();
        app.UseCors(CorsPolicy);
        app.UseRouting();
        app.UseCustomHangfire();

        app.UseEndpoints(x =>
        {
            x.MapControllers();
            x.MapHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
}
