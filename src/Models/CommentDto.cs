namespace web_backend.Models
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CommentBody { get; set; }
        public string Timestamp { get; set; } 

        public CommentDto(string id, string timestamp, string name, string commentBody)
        {
            Id = id;
            Name = name;
            CommentBody = commentBody;
            Timestamp = timestamp;
        }
    }
}
