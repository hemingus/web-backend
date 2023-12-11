using System.ComponentModel.DataAnnotations;

namespace web_backend.Entities
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [MaxLength(500)]
        public string PasswordHash { get; set; }
        public ICollection<TaskEntity> Tasks { get; set; }
        public string PartitionKey { get; set; } = "1";

        public User(string username, string passwordHash)
        {
            UserId = Guid.NewGuid().ToString();
            Username = username;
            PasswordHash = passwordHash;
            Tasks = new List<TaskEntity>();
        }
    }
}
