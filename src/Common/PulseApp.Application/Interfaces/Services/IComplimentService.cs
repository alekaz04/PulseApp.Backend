using PulseApp.Domain.Entities;

namespace PulseApp.Application.Interfaces;

/// <summary>
/// Сервис работы с комплиментами
/// </summary>
public interface IComplimentService
{
    /// <summary>
    /// Получить рандомный комплимент
    /// </summary>
    /// <param name="token">Токен отмены запроса</param>
    /// <returns>Комплимент</returns>
    /// <remarks>Полученный комплимент меняет свойство IsBeenPush на true</remarks>
    public Task<Compliment> GetRandomCompliment(CancellationToken token);

    /// <summary>
    /// Получить комплимент по идентификатору
    /// </summary>
    /// <param name="complimentId">Идентификатор комплимента</param>
    /// <param name="token">Токен отмены запроса</param>
    /// <returns>Комплимент</returns>
    Task<Compliment> GetComplimentById(Guid complimentId, CancellationToken token);
}
