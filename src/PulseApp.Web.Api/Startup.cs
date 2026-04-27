using Microsoft.EntityFrameworkCore;
using PulseApp.Application;
using PulseApp.Common.Extensions;
using PulseApp.Domain.Options;
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
            .AddApplicationServices(Configuration);

        services.AddDbContext<PulseDataContext>(x =>
            x.UseNpgsql(Configuration.GetConnectionString(nameof(PulseDataContext))));

        services.Configure<VapidOptions>(Configuration.GetSection(nameof(VapidOptions)));
        services.Configure<DatabaseOptions>(Configuration.GetSection(nameof(DatabaseOptions)));
        services.Configure<ApiKeyOption>(Configuration.GetSection(nameof(ApiKeyOption)));

        services.AddCustomHangfire(Configuration);
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

        app.UseApiProtection();

        app.UseCors(CorsPolicy);

        app.UseRouting();

        app.UseCustomHangfire();

        app.UseEndpoints(x =>
        {
            x.MapControllers();
        });
    }
}
