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

        // Save changes
        Task<bool> SaveChangesAsync();
    }
}
