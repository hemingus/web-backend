namespace web_backend.Models
{
    public class TaskEntityForCreationDto
    {
        public string Description { get; set; }
        public int Order { get; set; }
        public TaskEntityForCreationDto(string description, int order)
        {
            Description = description;
            Order = order;
        }
    }
}
