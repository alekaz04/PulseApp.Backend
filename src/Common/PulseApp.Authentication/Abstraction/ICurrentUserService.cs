using PulseApp.Domain.Entities;

namespace PulseApp.Authentication.Abstraction;

/// <summary>
/// Пользователь, который выполняет текущий запрос
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Получить идентфиикатор текущего пользователя
    /// </summary>
    /// <returns>Идентификатор</returns>
    public Guid GetCurrentUserId();

    /// <summary>
    /// Установить пользователя
    /// </summary>
    /// <remarks>Использовать только в Middleware</remarks>
    public User SetCurrentUser(User user);
}
