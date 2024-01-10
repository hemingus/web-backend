namespace web_backend.Entities
{
    public class Step
    {
        public string Id { get; set; }
        public string SubtaskId { get; set; }
        public string TaskId { get; set; }
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        public string Timestamp { get; set; }

        public Step(string taskId, string subtaskId, string description)
        {
            Id = Guid.NewGuid().ToString();
            SubtaskId = subtaskId;
            TaskId = taskId;
            Description = description;
            IsComplete = false;
            Timestamp = DateTime.Now.ToString();
        }
    }
}
