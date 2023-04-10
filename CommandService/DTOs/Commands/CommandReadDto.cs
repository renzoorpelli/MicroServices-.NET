using CommandService.Models;

namespace CommandService.DTOs.Commands;

public class CommandReadDto
{
    public Guid Id { get; set; }

    public string HowTo { get; set; } = null!;

    public string CommandLine { get; set; } = null!;
    public Guid PlatformId { get; set; }
    //public Platform Platform { get; set; }
}