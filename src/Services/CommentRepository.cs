using Microsoft.EntityFrameworkCore;
using System;
using web_backend.DbContexts;
using web_backend.Entities;

namespace web_backend.Services
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CosmosContext _context;
        public CommentRepository(CosmosContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //_context.Database.EnsureDeleted();
            //_context.Database.EnsureCreated();
        }
        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }
        public async Task<Comment?> GetCommentByIdAsync(string commentId)
        {
            try
            {
                return await _context.Comments
                    .SingleOrDefaultAsync(c => c.Id == commentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public void RemoveComment(Comment comment)
        {
            _context.Comments.Remove(comment);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
