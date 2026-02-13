using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;
using PulseApp.Hangfire;

namespace PulseApp.Application.Jobs;

/// <summary>
/// Job для раздачи комплиментов
/// </summary>
public class GiveComplimentJob : IHangfireRecurringJob
{
    /// <inheritdoc cref="IServiceProvider"/>
    private readonly IServiceProvider _serviceProvider;

    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<GiveComplimentJob> _logger;

    /// <inheritdoc/>
    /// <remarks>Крон каждый день с 8 до 22, в 26 минут</remarks>
    public string CronExpression { get; } = "26 8-22 * * *";

    /// <inheritdoc/>
    public RecurringJobOptions? JobOptions { get; } = new();


    public GiveComplimentJob(IServiceProvider serviceProvider, ILogger<GiveComplimentJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task Execute(CancellationToken token)
    {
        _logger.LogInformation("Start GiveComplimentJob");

        var complimentService = _serviceProvider.GetRequiredService<IComplimentService>();

        var compliment = await complimentService.GetRandomCompliment(token);

        var complimentPayload = new PushNotificationPayload(compliment.Title, compliment.Text);

        var pushNotificationService = _serviceProvider.GetRequiredService<IPushNotificationService>();

        await pushNotificationService.SendNotificationToAllSubscribes(complimentPayload, token);

        _logger.LogInformation("End GiveComplimentJob");
    }
}
