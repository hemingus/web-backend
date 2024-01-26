using web_backend.Entities;

namespace web_backend.Models
{
    public class SubtaskDto
    {
        public string TaskId { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        public string Timestamp { get; set; }
        public int Order { get; set; }
        public ICollection<Step> Steps { get; set; }

        public SubtaskDto(string taskId, string id, string timestamp, string description, Boolean isComplete, ICollection<Step> steps, int order)
        {
            TaskId = taskId;
            Id = id;
            Description = description;
            IsComplete = isComplete;
            Timestamp = timestamp;
            Steps = steps;
            Order = order;
        }
    }
}

