using System.ComponentModel.DataAnnotations;
using web_backend.Models;

namespace web_backend.Entities
{
    public class TaskEntity
    {
        public string Id { get; set; }
        [Required, MaxLength(500)]
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        public ICollection<Subtask> Subtasks { get; set; }
        public string Timestamp { get; set; }
        public int Order { get; set; }
        

        [Required]
        public string OwnerId { get; set; } = string.Empty;
        [Required]
        public string ProjectId { get; set; } = string.Empty;
        public string PartitionKey => OwnerId;

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
