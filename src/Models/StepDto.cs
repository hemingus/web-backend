namespace web_backend.Models
{
    public class StepDto
    {
        public string TaskId { get; set; }
        public string SubtaskId { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        public string Timestamp { get; set; }

        public StepDto(string taskId, string subtaskId, string id, string timestamp, string description, Boolean isComplete)
        {
            TaskId = taskId;
            SubtaskId = subtaskId;
            Id = id;
            Description = description;
            IsComplete = isComplete;
            Timestamp = timestamp;
        }
    }
}
