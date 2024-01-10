namespace web_backend.Models
{
    public class StepForCreationDto
    {
        public string Description { get; set; }

        public StepForCreationDto(string description)
        {
            Description = description;
        }
    }
}
