using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using PulseApp.Authentication.Abstraction;
using PulseApp.Domain.Entities;
using PulseApp.Infrastructure;

namespace PulseApp.Authentication.Services;

/// <inheritdoc/>
public class UserService(PulseDataContext context, ILogger<UserService> logger) : IUserService
{
    /// <summary>
    /// Имя пользователя, если в токене его нет
    /// </summary>
    private const string DefaultDisplayName = "Без имени";

    /// <inheritdoc cref="PulseDataContext"/>
    private readonly PulseDataContext _context = context;

    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<UserService> _logger = logger;

    /// <inheritdoc/>
    public async Task<User> GetOrCreateAsync(ClaimsPrincipal principal, CancellationToken token)
    {
        string keycloakId = principal.FindFirst(KeycloakClaimTypes.Subject)?.Value
                         ?? throw new InvalidOperationException($"Claim '{KeycloakClaimTypes.Subject}' is missing");
        string? email = principal.FindFirst(KeycloakClaimTypes.Email)?.Value;
        string displayName = principal.Identity?.Name ?? DefaultDisplayName;

        var user = await FindAsync(keycloakId, token);
        if (user is null)
        {
            return await CreateAsync(keycloakId, email, displayName, token);
        }

        if (user.Email == email && user.DisplayName == displayName)
        {
            return user;
        }

        await _context.Set<User>()
            .Where(u => u.Id == user.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Email, email)
                .SetProperty(u => u.DisplayName, displayName), token);

        user.Email = email;
        user.DisplayName = displayName;

        return user;
    }

    /// <summary>
    /// Найти пользователя по идентификатору Keycloak
    /// </summary>
    private Task<User?> FindAsync(string keycloakId, CancellationToken token)
    {
        return _context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.KeycloakId == keycloakId, token);
    }

    /// <summary>
    /// Создать пользователя. Если его уже создал параллельный запрос — вернуть существующего
    /// </summary>
    private async Task<User> CreateAsync(string keycloakId, string? email, string displayName, CancellationToken token)
    {
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            KeycloakId = keycloakId,
            Email = email,
            DisplayName = displayName,
            CreatedAt = DateTimeOffset.UtcNow,
        };

        _context.Set<User>().Add(newUser);
        try
        {
            await _context.SaveChangesAsync(token);
            _logger.LogInformation("Created user {UserId} for subject {KeycloakId}", newUser.Id, keycloakId);
            return newUser;
        }
        catch (DbUpdateException e) when (e.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            return await FindAsync(keycloakId, token)
                   ?? throw new InvalidOperationException($"User with subject '{keycloakId}' was not found after unique violation");
        }
        finally
        {
            // Контекст общий на весь запрос — не оставляем в нём пользователя
            _context.Entry(newUser).State = EntityState.Detached;
        }
    }
}
