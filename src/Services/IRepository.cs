using web_backend.Entities;

namespace web_backend.Services
{
    public interface IRepository
    {
        // Comment
        Task<IEnumerable<Comment>> GetCommentsAsync();
        Task<Comment> GetCommentByIdAsync(string commentId);
        void AddComment(Comment comment);
        void RemoveComment(Comment comment);
        // TaskEntity
        Task<IEnumerable<TaskEntity>> GetTasksAsync();
        Task<TaskEntity> GetTaskByIdAsync(string taskId);
        void UpdateTaskOrder(TaskEntity entity, int newOrder);
        void ReorderTasks();
        void AddTask(TaskEntity task);
        void RemoveTask(TaskEntity task);
        void UpdateTask(TaskEntity task);

        // Subtask
        IEnumerable<Subtask> GetSubtasks(TaskEntity task);
        Subtask GetSubtaskById(TaskEntity task, string subtaskId);
        void AddSubtask(TaskEntity task, Subtask subtask);
        void RemoveSubtask(TaskEntity task, string subtaskId);
        void UpdateSubtask(Subtask subtask);

        // Step
        IEnumerable<Step> GetSteps(Subtask subtask);
        Step GetStepById(Subtask subtask, string stepId);
        void AddStep(Subtask subtask, Step step);
        void RemoveStep(Subtask subtask, string stepId);
        void UpdateStep(Step step);

        // Save changes
        Task<bool> SaveChangesAsync();
    }
}
