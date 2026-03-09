namespace web_backend.Models.DTOs
{
    public class TaskEntityDto
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public int Order { get; set; }
        public IEnumerable<Subtask> Subtasks { get; set; }  

        public TaskEntityDto(string id, string projectId, DateTimeOffset timestamp, string description, Boolean isComplete, int order, IEnumerable<Subtask> subtasks)
        {
            Id = id;
            ProjectId = projectId;
            Description = description;
            IsComplete = isComplete;
            Timestamp = timestamp;
            Order = order;
            Subtasks = subtasks;
        }
    }
}
