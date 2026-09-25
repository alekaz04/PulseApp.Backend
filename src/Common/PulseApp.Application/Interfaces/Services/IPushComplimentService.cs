namespace PulseApp.Application.Services;

public interface IPushComplimentService
{
    Task SendComplimentToUser(Guid subscriptionId, Guid complimentId, CancellationToken token);
}
