namespace web_backend.Entities
{
    public class Subtask
    {
        public string Id { get; set; }
        public string TaskId { get; set; }
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        public ICollection<Step> Steps { get; set; }
        public string TimeStamp { get; set; }

        public Subtask(string taskId, string description)
        {
            Id = Guid.NewGuid().ToString();
            TaskId = taskId;
            Description = description;
            IsComplete = false;
            Steps = new List<Step>();
            TimeStamp = DateTime.Now.ToString();
        }
    }
}
