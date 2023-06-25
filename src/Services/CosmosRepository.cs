using Microsoft.EntityFrameworkCore;
using web_backend.DbContexts;
using web_backend.Entities;

namespace web_backend.Services
{
    public class CosmosRepository : IRepository
    {
        private readonly CosmosContext _context;

        public CosmosRepository(CosmosContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //_context.Database.EnsureDeleted();
            //_context.Database.EnsureCreated();
        }

         public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
      
    }
}
