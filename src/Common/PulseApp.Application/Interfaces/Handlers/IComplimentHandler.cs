using PulseApp.Application.DTOs;

namespace PulseApp.Application.Interfaces;

/// <summary>
/// Обработчик CRUD комплиментов
/// </summary>
public interface IComplimentHandler
{
    /// <summary>
    /// Создать комплимент
    /// </summary>
    Task<Guid> CreateCompliment(CreateComplimentDto complimentDto, CancellationToken token);

    /// <summary>
    /// Создать множество комплиментов
    /// </summary>
    Task<List<Guid>> CreateBatchCompliment(List<CreateComplimentDto> complimentDtos, CancellationToken token);

    /// <summary>
    /// Получить все комплименты
    /// </summary>
    Task<List<ComplimentDto>> GetAllCompliments(CancellationToken token);

    Task<ComplimentDto?> GetComplimentById(Guid complimentId, CancellationToken token);
}
