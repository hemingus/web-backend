using web_backend.Entities;

namespace web_backend.Services
{
    public interface ICommentRepository

    {
        // Comment
        Task<IEnumerable<Comment>> GetCommentsAsync();
        Task<Comment?> GetCommentByIdAsync(string commentId);
        void AddComment(Comment comment);
        void RemoveComment(Comment comment);

        // Save changes
        Task<bool> SaveChangesAsync();
    }
}
