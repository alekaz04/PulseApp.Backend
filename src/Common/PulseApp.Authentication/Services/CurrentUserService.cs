using PulseApp.Authentication.Abstraction;
using PulseApp.Common;
using PulseApp.Domain.Entities;

namespace PulseApp.Authentication.Services;

/// <inheritdoc/>
public class CurrentUserService : ICurrentUserService
{
    private User? CurrentUser { get; set; }

    public Guid GetCurrentUserId()
    {
        return CurrentUser?.Id ?? throw new CommonErrorException("Пользователь не найден");
    }

    public User SetCurrentUser(User user)
    {
        CurrentUser = user;
        return CurrentUser;
    }
}
