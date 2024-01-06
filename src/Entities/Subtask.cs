namespace web_backend.Entities
{
    public class Subtask
    {
        public string SubtaskId { get; set; }
        public string TaskId { get; set; }
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        public ICollection<Step> Steps { get; set; }
        public string TimeCreated { get; set; }
        public string PartitionKey { get; set; } = "2";

        public Subtask(string taskId, string description)
        {
            SubtaskId = Guid.NewGuid().ToString();
            TaskId = taskId;
            Description = description;
            IsComplete = false;
            Steps = new List<Step>();
        }
    }
}
