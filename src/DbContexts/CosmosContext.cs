using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos;
using web_backend.Entities;

namespace web_backend.DbContexts
{
    public class CosmosContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; } = null!;

        public CosmosContext(DbContextOptions<CosmosContext> options) : base(options)  
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasData(
                    new Comment("Ask", "Ka e det der igjen?"));

            modelBuilder.HasDefaultContainer("Comment").HasManualThroughput(400);

            modelBuilder.Entity<Comment>().HasKey(c => c.Id);

            modelBuilder.Entity<Comment>().HasPartitionKey(c => c.PartitionKey);

            base.OnModelCreating(modelBuilder);
        }
    }
}
