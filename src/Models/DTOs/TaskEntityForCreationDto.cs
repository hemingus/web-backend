namespace web_backend.Models.DTOs
{
    public class TaskEntityForCreationDto
    {
        public string Description { get; set; }
        public int Order { get; set; }
        public string ProjectId { get; set; }
        public TaskEntityForCreationDto(string description, int order, string projectId)
        {
            Description = description;
            Order = order;
            ProjectId = projectId;
        }
    }
}
