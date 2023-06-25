namespace web_backend.Models
{
    public class CommentForCreationDto
    {
        public string Name { get; set; }
        public string CommentBody { get; set; }

        public CommentForCreationDto(string name, string commentBody)
        {
            Name = name;
            CommentBody = commentBody;
        }
    }
}
