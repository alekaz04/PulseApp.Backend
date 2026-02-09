using Hangfire;

namespace PulseApp.Hangfire;

/// <summary>
/// Интерфейс для создания повторяющихся задач Hangfire
/// </summary>
public interface IHangfireRecurringJob
{
    /// <summary>
    /// Периодичность запуска выполнения заданий в Cron - формате
    /// </summary>
    public string CronExpression { get; }

    /// <summary>
    /// Параметры запуска заданий
    /// </summary>
    /// <remarks>Если не заданно, то по умолчанию используется настройка: <br/>
    /// <b>new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }</b>
    /// </remarks>
    public RecurringJobOptions? JobOptions { get; }

    /// <summary>
    /// Реализация логики выполнения задачи
    /// </summary>
    /// <param name="token">Токен отмены</param>
    public Task Execute(CancellationToken token);

    /// <summary>
    /// Идентификатор задачи
    /// </summary>
    /// <remarks>Если не реализовывать, то в качестве идентификатора будет использоваться имя типа</remarks>
    public string? JobId()
    {
        return null;
    }
}
