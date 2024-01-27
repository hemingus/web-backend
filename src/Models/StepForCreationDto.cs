namespace web_backend.Models
{
    public class StepForCreationDto
    {
        public string Description { get; set; }
        public int Order { get; set; }

        public StepForCreationDto(string description, int order)
        {
            Description = description;
            Order = order;
        }
    }
}
