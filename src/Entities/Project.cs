using System.ComponentModel.DataAnnotations;

namespace web_backend.Entities
{
    public class Project
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string OwnerId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; } = "Focus Director Project";
        public DateTimeOffset CreatedAt { get; set; }


        public Project(string name, string? description, string ownerId)
        {
            Id = Guid.NewGuid().ToString();
            OwnerId = ownerId;
            Name = name;
            Description = description;
            CreatedAt = DateTime.Now;
        }
    }
}
