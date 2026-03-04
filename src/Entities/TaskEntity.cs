using web_backend.Models;

namespace web_backend.Entities
{
    public class TaskEntity
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public DateTimeOffset Timestamp { get; private set; } = DateTime.UtcNow;
        public string Description { get; set; }
        public bool IsComplete { get; set; } = false;
        public int Order { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public string ProjectId { get; set; }
        public ICollection<Subtask> Subtasks { get; set; } = new List<Subtask>();

        public TaskEntity(string description, int order, string projectId)
        {
            Description = description;
            Order = order;
            ProjectId = projectId;
        }
    }
}