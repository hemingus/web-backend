using System.ComponentModel.DataAnnotations;
namespace web_backend.Entities
{
    public class User
    {
        [Required]
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        [Required]
        public string Email { get; private set; } = null!;
        [Required]
        public string Name { get; private set; } = null!;
        [Required]
        public string PasswordHash { get; private set; } = null!;

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }
        public DateTimeOffset? LastLoginAt { get; private set; }

        public User(string email, string name, string passwordHash)
        {
            Email = email;
            Name = name;
            PasswordHash = passwordHash;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void UpdateName(string name)
        {
            Name = name;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void RegisterLogin()
        {
            LastLoginAt = DateTimeOffset.UtcNow;
        }

        // New: set the hashed password in a controlled way
        public void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("passwordHash must not be empty", nameof(passwordHash));
            }

            PasswordHash = passwordHash;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        private User() { } // Required for EF Core
    }
}
