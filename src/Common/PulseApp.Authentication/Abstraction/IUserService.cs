using System.Security.Claims;
using PulseApp.Domain.Entities;

namespace PulseApp.Authentication.Abstraction;

/// <summary>
/// Сервис по работе с пользователем
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Получить или создать пользователя по <see cref="ClaimsPrincipal"/>.
    /// Email и имя синхронизируются с токеном
    /// </summary>
    /// <returns>Пользователь системы</returns>
    Task<User> GetOrCreateAsync(ClaimsPrincipal principal, CancellationToken token);
}
