namespace web_backend.Models
{
    public class SubtaskForCreationDto
    {
        public string Description { get; set; }

        public SubtaskForCreationDto(string description)
        {
            Description = description;
        }
    }
}
