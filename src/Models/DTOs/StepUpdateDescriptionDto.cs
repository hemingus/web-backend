namespace web_backend.Models.DTOs
{
    public class StepUpdateDescriptionDto
    {
        public string Description { get; set; }
        public StepUpdateDescriptionDto(string description)
        {
            Description = description;
        }
    }
}
