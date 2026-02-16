namespace web_backend.Models.DTOs
{
    public class StepUpdateCompletedDto
    {
        public bool IsComplete { get; set; }
        public StepUpdateCompletedDto(bool isComplete)
        {
            IsComplete = isComplete;
        }
    }
}
