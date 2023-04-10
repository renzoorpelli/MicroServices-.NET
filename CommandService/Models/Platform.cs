using System.ComponentModel.DataAnnotations;

namespace CommandService.Models;

public class Platform
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Publisher { get; set; } = null!;
    [Required]
    public string Cost { get; set; } = null!;
    [Required]
    public Guid ExternalId { get; set; }
    public ICollection<Command> Commands { get; set; } = new List<Command>();
}