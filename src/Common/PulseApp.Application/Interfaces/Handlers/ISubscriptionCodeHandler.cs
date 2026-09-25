namespace PulseApp.Application.Controllers;

public interface ISubscriptionCodeHandler
{
    /// <summary>
    /// Создать код для подписки
    /// </summary>
    /// <param name="createCodeDto">Дто создания кода</param>
    /// <param name="token">Токен отмены запросы</param>
    /// <returns>Код</returns>
    Task<string> CreateSubscriptionCode(CreateSubscriptionCodeDto createCodeDto, CancellationToken token);
}
