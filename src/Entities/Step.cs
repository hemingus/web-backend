using System.ComponentModel.DataAnnotations;

namespace web_backend.Entities
{
    public class Step
    {
        [Key]
        public string Id { get; set; }
        public string SubtaskId { get; set; }
        public string TaskId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        [DataType(DataType.DateTime)]
        public string Timestamp { get; set; }
        public int Order { get; set; }

        public Step(string taskId, string subtaskId, string description, int order)
        {
            Id = Guid.NewGuid().ToString();
            SubtaskId = subtaskId;
            TaskId = taskId;
            Description = description;
            IsComplete = false;
            Timestamp = DateTime.Now.ToString();
            Order = order;
        }
    }
}
