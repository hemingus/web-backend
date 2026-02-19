using web_backend.Models;

public class TaskEntity
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public DateTimeOffset Timestamp { get; private set; } = DateTime.UtcNow;
    public string Description { get; private set; }
    public bool IsComplete { get; private set; } = false;
    public int Order { get; private set; }
    public string OwnerId { get; private set; }
    public string ProjectId { get; private set; }
    public ICollection<Subtask> Subtasks { get; private set; } = new List<Subtask>();

    public TaskEntity(string description, int order, string ownerId, string projectId)
    {
        Description = description;
        Order = order;
        OwnerId = ownerId;
        ProjectId = projectId;
    }
}