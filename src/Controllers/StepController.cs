using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Collections.Specialized;
using web_backend.Entities;
using web_backend.Models;
using web_backend.Services;

namespace web_backend.Controllers
{
    [ApiController]
    [Route("taskentity/{taskId}/subtask/{subtaskId}/step")]
    public class StepController : Controller
    {
        private readonly IRepository _repo;
        private readonly NameValueCollection settings =
            System.Configuration.ConfigurationManager.AppSettings;
        public StepController(IRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStepById(string taskId, string subtaskId, string id)
        {
            try
            {
                var task = await _repo.GetTaskByIdAsync(taskId);
                if (task == null)
                {
                    return NotFound(); // TaskEntity with the provided ID not found
                }
                var subtask = _repo.GetSubtaskById(task, subtaskId);
                if (subtask == null)
                {
                    return NotFound(); // Subtask with the provided ID not found
                }
                var step = _repo.GetStepById(subtask, id);
                return Ok(step);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet(Name = "GetSteps")]
        public async Task<IActionResult> GetAllSteps(string taskId, string subtaskId)
        {
            try
            {
                var task = await _repo.GetTaskByIdAsync(taskId);
                if (task == null)
                {
                    return NotFound(); // TaskEntity with the provided ID not found
                }
                var subtask = _repo.GetSubtaskById(task, subtaskId);
                if (subtask == null)
                {
                    return NotFound(); // Subtask with the provided ID not found
                }
                var steps = _repo.GetSteps(subtask);
                return Ok(steps);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch]
        public async Task<ActionResult<CommentDto>> CreateStep(string taskId, string subtaskId,
            StepForCreationDto stepForCreation)
        {
            try
            {
                var task = await _repo.GetTaskByIdAsync(taskId);
                if (task == null)
                {
                    return NotFound(); // TaskEntity with the provided ID not found
                }
                var subtask = _repo.GetSubtaskById(task, subtaskId);
                if (subtask == null)
                {
                    return NotFound(); // Subtask with the provided ID not found
                }
                Step step = new Step(taskId, subtaskId, stepForCreation.Description);
                _repo.AddStep(subtask, step);
                await _repo.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStep(string taskId, string subtaskId,
            string id)
        {
            try
            {
                var task = await _repo.GetTaskByIdAsync(taskId);
                if (task == null)
                {
                    return NotFound(); // TaskEntity with the provided ID not found
                }
                var subtask = _repo.GetSubtaskById(task, subtaskId);
                if (subtask == null)
                {
                    return NotFound(); // Subtask with the provided ID not found
                }
                _repo.RemoveStep(subtask, id);
                await _repo.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            } 
        }
    }
}
