using PulseApp.Application.Controllers;

namespace PulseApp.Application.Interfaces.Services;

public interface ISubscriptionCodeService
{
    /// <summary>
    /// Создать код для подписки
    /// </summary>
    /// <param name="createCodeDto"></param>
    /// <param name="token">Токен отмены запроса</param>
    /// <returns>Код</returns>
    Task<string> CreateSubscriptionCode(CreateSubscriptionCodeDto createCodeDto, CancellationToken token);
}
