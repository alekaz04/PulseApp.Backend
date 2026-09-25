namespace PulseApp.Application.DTOs;

public class SubscriptionDto
{
    public Guid Id { get; set; }
    public string? UserAgent { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
