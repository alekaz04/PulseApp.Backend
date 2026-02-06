using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NpgsqlTypes;
using PulseApp.Common;
using PulseApp.Logging.Models;
using PulseApp.Logging.PostgresLoggerConfiguration.ColumnWriters;
using Serilog;
using Serilog.Sinks.PostgreSQL.ColumnWriters;

namespace PulseApp.Logging.Extensions;

/// <summary>
/// Класс расширений <see cref="IServiceCollection"/> для модуля логирования
/// </summary>
public static class LoggingServiceCollectionExtensions
{
    /// <summary>
    /// Добавить логгирование
    /// </summary>
    public static IServiceCollection AddPostgresLogging(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Logs")
                                  ?? throw new CommonErrorException("Connection string not found");

        IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            {nameof(LogEntity.Id), new GuidColumnWriter(NpgsqlDbType.Uuid) },
            {nameof(LogEntity.Level), new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
            {nameof(LogEntity.RaiseDate), new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
            {nameof(LogEntity.Message), new RenderedMessageColumnWriter(NpgsqlDbType.Text)  },
            {nameof(LogEntity.Exception), new ExceptionColumnWriter(NpgsqlDbType.Text) },
            {nameof(LogEntity.LogEventProperties), new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) }
        };

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.PostgreSQL(connectionString,
                "Logs",
                columnWriters,
                schemaName: "public",
                needAutoCreateTable: true,
                failureCallback: ex => Console.WriteLine($"Sink error: {ex.Message}"))
            .WriteTo.Console()
            .Enrich.FromLogContext()
            .CreateLogger();

        services.AddLogging()
            .AddSerilog(logger);

        return services;
    }
}
