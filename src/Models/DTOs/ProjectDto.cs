namespace web_backend.Models.DTOs
{
    public class ProjectDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public ProjectDto(string id, string name, string? description, DateTimeOffset createdAt)
        {
            Id = id;
            Name = name;
            Description = description;
            CreatedAt = createdAt;
        }
    }
}
