using Hangfire;
using PulseApp.Hangfire;

namespace PulseApp.Application.Jobs;

/// <summary>
/// Job для раздачи комплиментов
/// </summary>
public class GiveComplimentJob : IHangfireRecurringJob
{
    /// <inheritdoc/>
    public string CronExpression { get; } = string.Empty;

    /// <inheritdoc/>
    public RecurringJobOptions? JobOptions { get; } = new();

    public async Task Execute(CancellationToken token)
    {
    }
}
