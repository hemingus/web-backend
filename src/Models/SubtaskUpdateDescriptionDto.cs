namespace web_backend.Models
{
    public class SubtaskUpdateDescriptionDto
    {
        public string Description { get; set; }
        public SubtaskUpdateDescriptionDto(string description)
        {
            Description = description;
        }
    }
}
