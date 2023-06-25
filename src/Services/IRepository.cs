using web_backend.Entities;

namespace web_backend.Services
{
    public interface IRepository
    {
        Task<IEnumerable<Comment>> GetCommentsAsync();
        void AddComment(Comment comment);
        Task<bool> SaveChangesAsync();
    }
}
