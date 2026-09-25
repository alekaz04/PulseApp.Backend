using AutoMapper;
using PulseApp.Authentication.Abstraction;

namespace PulseApp.Application.AutoMapper;

/// <summary>
/// Резолвер идентификатора пользователя, выполняющего текущий запрос
/// </summary>
public class CurrentUserIdResolver(ICurrentUserService currentUserService) : IValueResolver<object, object, Guid>
{
    /// <inheritdoc cref="ICurrentUserService"/>
    private readonly ICurrentUserService _currentUserService = currentUserService;

    /// <inheritdoc/>
    public Guid Resolve(object source, object destination, Guid destMember, ResolutionContext context)
    {
        return _currentUserService.GetCurrentUserId();
    }
}
