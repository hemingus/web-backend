namespace web_backend.Models
{
    public class SubtaskForCreationDto
    {
        public string Description { get; set; }
        public int Order { get; set; }

        public SubtaskForCreationDto(string description, int order)
        {
            Description = description;
            Order = order;
        }
    }
}
