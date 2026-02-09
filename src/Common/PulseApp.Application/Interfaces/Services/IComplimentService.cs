using PulseApp.Domain.Entities;

namespace PulseApp.Application.Interfaces;

/// <summary>
/// Сервис работы с комплиментами
/// </summary>
public interface IComplimentService
{
    /// <summary>
    /// Создать комплимент
    /// </summary>
    /// <param name="newCompliment">Новый комплимент</param>
    /// <param name="token">Токен отмены запроса</param>
    Task CreateCompliment(Compliment newCompliment, CancellationToken token);

    /// <summary>
    /// Создать много комплиментов
    /// </summary>
    /// <param name="compliments">Список комплиментов</param>
    /// <param name="token">Токен отмены запроса</param>
    public Task CreateBatchCompliment(List<Compliment> compliments, CancellationToken token);

    /// <summary>
    /// Получить рандомный комплимент
    /// </summary>
    /// <param name="token">Токен отмены запроса</param>
    /// <returns>Комплимент</returns>
    /// <remarks>Полученный комплимент меняет свойство IsBeenPush на true</remarks>
    public Task<Compliment> GetRandomCompliment(CancellationToken token);
}
