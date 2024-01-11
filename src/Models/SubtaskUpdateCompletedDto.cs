namespace web_backend.Models
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
