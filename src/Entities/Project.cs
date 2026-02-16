using System.ComponentModel.DataAnnotations;

namespace web_backend.Entities
{
    public class Project
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public Project(string name, string? description)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Description = description;
            CreatedAt = DateTime.Now;
        }
    }
}
