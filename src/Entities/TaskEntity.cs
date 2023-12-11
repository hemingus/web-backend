namespace web_backend.Entities
{
    public class TaskEntity
    {
        public string TaskId { get; set; }
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        public ICollection<Subtask> Subtasks { get; set; }
        public string Created { get; set; }

        public TaskEntity(string description, Boolean isComplete)
        {
            Description = description;
            IsComplete = isComplete;
            Subtasks = new List<Subtask>();
            TaskId = Guid.NewGuid().ToString();
            Created = DateTime.Now.ToString();
        }

    }
}
