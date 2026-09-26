namespace PulseApp.Application.DTOs;

public class SubscriptionDto
{
    public Guid Id { get; set; }
    public string? UserAgent { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
