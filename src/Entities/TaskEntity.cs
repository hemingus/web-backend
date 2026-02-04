namespace web_backend.Entities
{
    public class TaskEntity
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        public ICollection<Subtask> Subtasks { get; set; }
        public string Timestamp { get; set; }
        public int Order { get; set; }
        public string PartitionKey { get; set; } = "2";

        // New: owner identifier to associate tasks with a user
        public string OwnerId { get; set; } = string.Empty;

        public TaskEntity(string description, Boolean isComplete, int order)
        {
            Description = description;
            IsComplete = isComplete;
            Subtasks = new List<Subtask>();
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now.ToString();
            Order = order;
        }
    }
}
