using Microsoft.Extensions.Options;
using PulseApp.Application.Interfaces;
using PulseApp.Domain.Options;

namespace PulseApp.Application.Services;

/// <summary>
/// Реализация сервиса для работы с VAPID ключами
/// </summary>
public class VapidService : IVapidService
{
    /// <inheritdoc cref="VapidOptions"/>
    private readonly VapidOptions _options;

    public VapidService(IOptions<VapidOptions> options)
    {
        _options = options.Value;
    }

    /// <inheritdoc/>
    public VapidOptions GetVapidKeys()
    {
        return _options;
    }

    /// <inheritdoc/>
    public string GetPublicKeyAsync()
    {
        return _options.PublicKey;
    }
}
