namespace web_backend.Models.DTOs
{
    public class TaskEntityUpdateDescriptionDto
    {
        public string Description { get; set; }
        public TaskEntityUpdateDescriptionDto(string description)
        {
            Description = description;
        }
    }
}
