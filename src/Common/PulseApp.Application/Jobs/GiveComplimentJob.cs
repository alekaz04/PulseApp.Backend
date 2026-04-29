using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;
using PulseApp.Common;
using PulseApp.Domain.Entities;
using PulseApp.Domain.Options;
using PulseApp.Hangfire;

namespace PulseApp.Application.Jobs;

/// <summary>
/// Job для раздачи комплиментов
/// </summary>
public class GiveComplimentJob : IHangfireRecurringJob
{
    /// <inheritdoc cref="IServiceScopeFactory"/>
    private readonly IServiceScopeFactory _scopeFactory;

    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<GiveComplimentJob> _logger;

    /// <inheritdoc cref="IOptions{T}"/>
    private readonly IOptions<SchedulerOptions> _options;

    /// <inheritdoc/>
    public string CronExpression { get; }

    /// <inheritdoc/>
    public RecurringJobOptions? JobOptions { get; } = new();


    public GiveComplimentJob(IServiceScopeFactory scopeFactory, ILogger<GiveComplimentJob> logger, IOptions<SchedulerOptions> options)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _options = options;
        CronExpression = _options.Value.ComplimentCronExpression;
    }

    public async Task Execute(CancellationToken token)
    {
        _logger.LogInformation("Start GiveComplimentJob");

        using var scope = _scopeFactory.CreateScope();

        var complimentService = scope.ServiceProvider.GetRequiredService<IComplimentService>();

        Compliment? compliment;
        try
        {
            compliment = await complimentService.GetRandomCompliment(token);
        }
        catch (CommonErrorException ex)
        {
            _logger.LogWarning("Compliment pool is empty, skipping: {Message}", ex.Message);
            return;
        }

        var complimentPayload = new PushNotificationPayload(compliment.Title, compliment.Text);

        var pushNotificationService = scope.ServiceProvider.GetRequiredService<IPushNotificationService>();

        await pushNotificationService.SendNotificationToAllSubscribes(complimentPayload, token);

        _logger.LogInformation("End GiveComplimentJob");
    }
}
