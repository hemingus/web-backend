namespace web_backend.Models
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
