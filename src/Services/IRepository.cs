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
        void AddTask(TaskEntity task);
        void RemoveTask(TaskEntity task);
        void UpdateTask(TaskEntity task);

        // Subtask
        IEnumerable<Subtask> GetSubtasks(TaskEntity task);
        Subtask GetSubtaskById(TaskEntity task, string subtaskId);
        void AddSubtask(TaskEntity task, Subtask subtask);
        void RemoveSubtask(TaskEntity task, string subtaskId);
        void UpdateSubtask(Subtask subtask);

        // Save changes
        Task<bool> SaveChangesAsync();
    }
}
