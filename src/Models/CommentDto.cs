namespace web_backend.Models
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CommentBody { get; set; }

        public CommentDto(int id, string name, string commentBody)
        {
            Id = id;
            Name = name;
            CommentBody = commentBody;
        }
    }
}
