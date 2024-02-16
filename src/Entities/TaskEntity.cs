using System.ComponentModel.DataAnnotations;

namespace web_backend.Entities
{
    public class TaskEntity
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        public ICollection<Subtask> Subtasks { get; set; }
        [DataType(DataType.DateTime)]
        public string Timestamp { get; set; }
        public int Order { get; set; }
        public string PartitionKey { get; set; } = "2";

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
