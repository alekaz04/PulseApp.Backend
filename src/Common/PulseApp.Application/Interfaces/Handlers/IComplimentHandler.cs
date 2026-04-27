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

    /// <summary>
    /// Получить по идентификатору
    /// </summary>
    Task<ComplimentDto?> GetComplimentById(Guid complimentId, CancellationToken token);

    /// <summary>
    /// Обновить по идентификатору
    /// </summary>
    Task UpdateComplimentById(Guid complimentId, ComplimentUpdateDto complimentUpdateDto, CancellationToken token);

    /// <summary>
    /// Удалить по идентификатору
    /// </summary>
    Task DeleteComplimentById(Guid complimentId, CancellationToken token);
}
