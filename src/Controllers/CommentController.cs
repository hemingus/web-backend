using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Collections.Specialized;
using web_backend.Entities;
using web_backend.Models;
using web_backend.Services;

namespace web_backend.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class CommentController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly NameValueCollection settings = 
            System.Configuration.ConfigurationManager.AppSettings;

        public CommentController(IRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        private IEnumerable<CommentDto> generateComments()
        {
            List<CommentDto> comments = new List<CommentDto>();
            comments.Add(new CommentDto("1", "Alf", "Hallais!"));
            comments.Add(new CommentDto("2", "Gunn", "Heisann!"));

            return comments;
        }

        [HttpGet(Name = "GetComments")]
        public async Task<IEnumerable<CommentDto>> GetAllComments()
        {
            var commentsFromDb = await _repo.GetCommentsAsync();
            var commentDtosToReturn = new List<CommentDto>();
            foreach (var comment in commentsFromDb)
            {
                commentDtosToReturn.Add(new CommentDto(comment.Id, comment.Name, comment.CommentBody));
            }
            return commentDtosToReturn;
        }

        [HttpPost]
        public async Task<ActionResult<CommentDto>> createComment(
            CommentForCreationDto comment)
        {
            var commentEntity = new Comment(comment.Name, comment.CommentBody);
            _repo.AddComment(commentEntity);
            await _repo.SaveChangesAsync();
            var commentToReturn = new CommentDto(commentEntity.Id, commentEntity.Name, commentEntity.CommentBody);
            return CreatedAtRoute("GetComments", commentToReturn);
        }
    }
}
