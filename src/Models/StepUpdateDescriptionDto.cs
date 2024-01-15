namespace web_backend.Models
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
