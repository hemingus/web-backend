using System.ComponentModel.DataAnnotations;
using web_backend.Models;

public class TaskEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required, MaxLength(500)]
    public string Description { get; set; } = null!;

    public bool IsComplete { get; set; }

    public ICollection<Subtask> Subtasks { get; set; } = new List<Subtask>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int Order { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    public string ProjectId { get; set; } = null!;
}
