namespace web_backend.Entities
{
    public class Subtask
    {
        public string TaskId { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        public string Timestamp { get; set; }
        public int Order { get; set; }
        public ICollection<Step> Steps { get; set; }

        public Subtask(string taskId, string description, int order)
        {
            Id = Guid.NewGuid().ToString();
            TaskId = taskId;
            Description = description;
            IsComplete = false;
            Steps = new List<Step>();
            Timestamp = DateTime.Now.ToString();
            Order = order;
        }
    }
}
