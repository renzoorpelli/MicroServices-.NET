namespace PlatformService.DTOs.Platform;

public class PlatformPublishDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Publisher { get; set; } = null!;
    public string Cost { get; set; } = null!;
    public Event Event { get; set; }
}

public enum Event
{
    PlatformPublished = 1,
    Undeterminated = 0
}