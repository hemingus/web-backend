namespace web_backend.Models.DTOs
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
