using AutoMapper;
using PulseApp.Authentication.Abstraction;
using PulseApp.Common;

namespace PulseApp.Application.AutoMapper;

/// <summary>
/// Резолвер идентификатора пользователя, выполняющего текущий запрос
/// </summary>
public class CurrentUserIdResolver : IValueResolver<object, object, Guid>
{
    /// <inheritdoc cref="ICurrentUserService"/>
    private readonly ICurrentUserService _currentUserService;

    public CurrentUserIdResolver(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    /// <inheritdoc/>
    public Guid Resolve(object source, object destination, Guid destMember, ResolutionContext context)
    {
        return _currentUserService.CurrentUser?.Id ??
               throw new CommonErrorException("Текущий пользователь не найден");
    }
}
