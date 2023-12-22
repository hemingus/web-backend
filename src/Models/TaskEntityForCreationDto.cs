namespace web_backend.Models
{
    public class TaskEntityForCreationDto
    {
        public string Description { get; set; }
        
        public TaskEntityForCreationDto(string description)
        {
            Description = description;
        }
    }
}
