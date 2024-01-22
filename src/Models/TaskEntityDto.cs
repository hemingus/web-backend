using web_backend.Entities;

namespace web_backend.Models
{
    public class TaskEntityDto
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        public string Timestamp { get; set; }
        public int Order { get; set; }
        public ICollection<Subtask> Subtasks { get; set; }  

        public TaskEntityDto(string id, string timestamp, string description, Boolean isComplete, int order, ICollection<Subtask> subtasks)
        {
            Id = id;
            Description = description;
            IsComplete = isComplete;
            Timestamp = timestamp;
            Order = order;
            Subtasks = subtasks;
            
        }
    }
}
