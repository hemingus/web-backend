using Microsoft.EntityFrameworkCore;
using web_backend.Entities;

namespace web_backend.DbContexts
{
    public class CosmosContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<TaskEntity> Tasks { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;

        public CosmosContext(DbContextOptions<CosmosContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // USERS
            // =========================

            modelBuilder.Entity<User>()
                .ToContainer("Users")
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasPartitionKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();


            // =========================
            // PROJECTS
            // =========================

            modelBuilder.Entity<Project>()
                .ToContainer("Projects")
                .HasKey(p => p.Id);

            modelBuilder.Entity<Project>()
                .HasPartitionKey(p => p.OwnerId);

            modelBuilder.Entity<Project>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<Project>()
                .Property(p => p.OwnerId)
                .IsRequired();


            // =========================
            // TASKS
            // =========================

            modelBuilder.Entity<TaskEntity>()
                .ToContainer("Tasks")
                .HasKey(t => t.Id);

            modelBuilder.Entity<TaskEntity>()
                .HasPartitionKey(t => t.OwnerId);

            modelBuilder.Entity<TaskEntity>()
                .Property(t => t.Description)
                .IsRequired();

            modelBuilder.Entity<TaskEntity>()
                .Property(t => t.OwnerId)
                .IsRequired();

            modelBuilder.Entity<TaskEntity>()
                .Property(t => t.ProjectId)
                .IsRequired();


            // =========================
            // COMMENTS (Optional)
            // =========================

            modelBuilder.Entity<Comment>()
                .ToContainer("Comments")
                .HasKey(c => c.Id);

            modelBuilder.Entity<Comment>()
                .HasPartitionKey(c => c.OwnerId);
        }
    }
}
