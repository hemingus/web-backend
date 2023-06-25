using System.ComponentModel.DataAnnotations;

namespace web_backend.Entities
{
    public class Comment
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string CommentBody { get; set; }
        [DataType(DataType.DateTime)]
        public string Timestamp { get; set; }
        public string PartitionKey { get; set; } = "1";

        public Comment(string name, string commentBody)
        {
            Name = name;
            CommentBody = commentBody;
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now.ToString();
        }
    }
}
