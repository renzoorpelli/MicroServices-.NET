namespace CommandService.DTOs.Platforms;

public class PlatformReadDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid ExternalId { get; set; }
}