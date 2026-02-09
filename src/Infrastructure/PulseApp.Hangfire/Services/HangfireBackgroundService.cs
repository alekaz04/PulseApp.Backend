using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PulseApp.Hangfire;

/// <summary>
/// Сервис регистрации задач для Hangfire
/// </summary>
public class HangfireBackgroundService : BackgroundService
{
    /// <inheritdoc cref="IHangfireRecurringJob"/>
    private readonly IEnumerable<IHangfireRecurringJob> _jobs;

    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<HangfireBackgroundService> _logger;

    public HangfireBackgroundService(IEnumerable<IHangfireRecurringJob> jobs, ILogger<HangfireBackgroundService> logger)
    {
        _jobs = jobs;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            CleanJobs();
            RegisterRecursiveJob();
        }
        catch (OperationCanceledException)
        {
            // ignored
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on init Hangfire");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Очищает контейнер ранее зарегистрированных задач
    /// </summary>
    private static void CleanJobs()
    {
        using var connection = JobStorage.Current.GetConnection();

        foreach (var recurringJob in connection.GetRecurringJobs())
        {
            RecurringJob.RemoveIfExists(recurringJob.Id);
        }
    }

    /// <summary>
    /// Регистрирует повторяющиеся задачи
    /// </summary>
    private void RegisterRecursiveJob()
    {
        foreach (var job in _jobs)
        {
            RecurringJob.AddOrUpdate(job.JobId() ?? job.GetType().FullName,
                () => job.Execute(CancellationToken.None),
                job.CronExpression,
                job.JobOptions ?? new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });
        }
    }
}
