using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using web_backend.Models;

namespace web_backend.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class CommentController : ControllerBase
    {
        

        private IEnumerable<CommentDto> generateComments()
        {
            List<CommentDto> comments = new List<CommentDto>();
            comments.Add(new CommentDto(1, "Alf", "Hallais!"));
            comments.Add(new CommentDto(2, "Gunn", "Heisann!"));

            return comments;
        }


        [HttpGet]
        public async Task<IEnumerable<CommentDto>> getAllComments()
        {
            var settings = System.Configuration.ConfigurationManager.AppSettings;
            //return generateComments();
            using CosmosClient client = 
                new(accountEndpoint: settings["CosmosDbUrl"],
                authKeyOrResourceToken: settings["CosmosDbSecret"]
                );

            var container = client.GetContainer(
                settings["CosmosDatabaseName"], 
                settings["CosmosContainerName"]
                );

            using FeedIterator<CommentDto> feed = container.GetItemQueryIterator<CommentDto>(
                queryText: "SELECT * FROM comments WHERE comments.name = 'Severin'"
                );

            while (feed.HasMoreResults)
            {
                FeedResponse<CommentDto> response = await feed.ReadNextAsync();

                return response;

            }
            return null; 

        }
    }
}
