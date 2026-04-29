using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PulseApp.Application.DTOs;
using PulseApp.Application.Interfaces;
using PulseApp.Common;
using PulseApp.Domain.Entities;
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

    /// <inheritdoc/>
    /// <remarks>Крон каждый день с 8 до 22, в 26 минут</remarks>
    public string CronExpression { get; } = "26 8-22 * * *";

    /// <inheritdoc/>
    public RecurringJobOptions? JobOptions { get; } = new();


    public GiveComplimentJob(IServiceScopeFactory scopeFactory, ILogger<GiveComplimentJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
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
