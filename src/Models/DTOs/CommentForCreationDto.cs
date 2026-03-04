namespace web_backend.Models.DTOs
{
    public class CommentForCreationDto
    {
        public string Name { get; set; }
        public string CommentBody { get; set; }

        public string OwnerId { get; private set; }

        public CommentForCreationDto(string name, string commentBody, string ownerId)
        {
            Name = name;
            CommentBody = commentBody;
            OwnerId = ownerId;
        }
    }
}
