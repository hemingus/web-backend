namespace web_backend.Entities
{
    public class TaskEntity
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        public ICollection<Subtask> Subtasks { get; set; }
        public string Timestamp { get; set; }
        public string PartitionKey { get; set; } = "2";

        public TaskEntity(string description, Boolean isComplete)
        {
            Description = description;
            IsComplete = isComplete;
            Subtasks = new List<Subtask>();
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now.ToString();
        }
    }
}
