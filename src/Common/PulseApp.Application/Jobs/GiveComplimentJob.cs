using Hangfire;
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
    /// <inheritdoc cref="IComplimentService"/>
    private readonly IComplimentService _complimentService;

    /// <inheritdoc cref="IPushNotificationService"/>
    private readonly IPushNotificationService _pushNotificationService;

    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<GiveComplimentJob> _logger;

    /// <inheritdoc/>
    /// <remarks>Крон каждый день с 8 до 22, в 26 минут</remarks>
    public string CronExpression { get; } = "26 8-22 * * *";

    /// <inheritdoc/>
    public RecurringJobOptions? JobOptions { get; } = new();


    public GiveComplimentJob(IComplimentService complimentService, IPushNotificationService pushNotificationService, ILogger<GiveComplimentJob> logger)
    {
        _complimentService = complimentService;
        _pushNotificationService = pushNotificationService;
        _logger = logger;
    }

    public async Task Execute(CancellationToken token)
    {
        _logger.LogInformation("Start GiveComplimentJob");
        var compliment = await _complimentService.GetRandomCompliment(token);

        var complimentPayload = new PushNotificationPayload(compliment.Title, compliment.Text);

        await _pushNotificationService.SendNotificationToAllSubscribes(complimentPayload, token);
        _logger.LogInformation("End GiveComplimentJob");
    }
}
