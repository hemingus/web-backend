namespace web_backend.Models.DTOs
{
    public class TaskEntityUpdateCompletedDto
    {
        public bool IsComplete { get; set; }

        public TaskEntityUpdateCompletedDto(bool isComplete)
        {
            IsComplete = isComplete;
        }
    }
}
