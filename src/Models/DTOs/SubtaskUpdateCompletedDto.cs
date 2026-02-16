namespace web_backend.Models.DTOs
{
    public class SubtaskUpdateCompletedDto
    {
        public bool IsComplete { get; set; }
        public SubtaskUpdateCompletedDto(bool isComplete)
        {
            IsComplete = isComplete;
        }
    }
}
