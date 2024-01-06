namespace web_backend.Models
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
