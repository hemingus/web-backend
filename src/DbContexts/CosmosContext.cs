using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos;
using web_backend.Entities;

namespace web_backend.DbContexts
{
    public class CosmosContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<TaskEntity> Tasks { get; set; } = null!;

        public CosmosContext(DbContextOptions<CosmosContext> options) : base(options)  
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Comments

            modelBuilder.Entity<Comment>()
                .HasData(
                    new Comment("Ask", "Ka e det der igjen?"));

            //modelBuilder.HasDefaultContainer("Comment").HasManualThroughput(400);
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Comment>().ToContainer("Comment");
            modelBuilder.Entity<Comment>().HasPartitionKey(c => c.PartitionKey);

            // Tasks
            modelBuilder.Entity<TaskEntity>()
                .HasData(
                    new TaskEntity("My first task from DB", false));
            modelBuilder.HasDefaultContainer("TaskEntity").HasManualThroughput(400);
            modelBuilder.Entity<TaskEntity>().HasKey(t => t.Id);
            modelBuilder.Entity<TaskEntity>().ToContainer("TaskEntity");
            modelBuilder.Entity<TaskEntity>().HasPartitionKey(t => t.PartitionKey);

            base.OnModelCreating(modelBuilder);
        }
    }
}
