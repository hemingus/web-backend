using System.ComponentModel.DataAnnotations;

namespace web_backend.Entities
{
    public class User
    {
        [Key]
        public string UserId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string PasswordHash { get; set; } = null!;
    }
}
