using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Collections.Specialized;
using web_backend.Entities;
using web_backend.Models;
using web_backend.Services;

namespace web_backend.Controllers
{
    [ApiController]
    [Route("taskentity/{taskId}/subtask")]
    public class SubtaskController : Controller
    {
        private readonly IRepository _repo;
        private readonly NameValueCollection settings =
            System.Configuration.ConfigurationManager.AppSettings;
        public SubtaskController(IRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [HttpGet("{subtaskId}")]
        public async Task<IActionResult> GetSubtaskById(string taskId, string subtaskId)
        {
            try
            {
                var task = await _repo.GetTaskByIdAsync(taskId);
                if (task == null)
                {
                    return NotFound();
                }
                var subtask = _repo.GetSubtaskById(task, subtaskId);
                var subtaskToReturn = new SubtaskDto(
                    subtask.TaskId, subtask.Id, subtask.Timestamp, subtask.Description, 
                    subtask.IsComplete, subtask.Steps, subtask.Order);
                
                return Ok(subtaskToReturn);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }

        [HttpGet(Name = "GetSubtasks")]
        public async Task<IActionResult> GetAllSubtasks(string taskId)
        {
            try
            {
                var task = await _repo.GetTaskByIdAsync(taskId);
                if (task == null)
                {
                    return NotFound(); // TaskEntity with the provided ID not found
                }
                var subtasks = _repo.GetSubtasks(task);
                return Ok(subtasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch]
        public async Task<ActionResult<SubtaskDto>> CreateSubtask(string taskId,
            SubtaskForCreationDto subtaskForCreation)
        {
            try
            {
                var task = await _repo.GetTaskByIdAsync(taskId);
                if (task == null)
                {
                    return NotFound();
                }
                var subtask = new Subtask(taskId, subtaskForCreation.Description, subtaskForCreation.Order);
                _repo.AddSubtask(task, subtask);
                await _repo.SaveChangesAsync();
                return NoContent();
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{subtaskId}")]
        public async Task<IActionResult> DeleteSubtask(string taskId,
            string subtaskId)
        {
            try
            {
                var task = await _repo.GetTaskByIdAsync(taskId);
                if (task == null)
                {
                    return NotFound();
                }
                _repo.RemoveSubtask(task, subtaskId);
                await _repo.SaveChangesAsync();
                _repo.ReorderSubtasks(task);
                await _repo.SaveChangesAsync();
                return NoContent();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch("{subtaskId}")]
        public async Task<IActionResult> UpdateSubtaskCompleted(string taskId, string subtaskId, SubtaskUpdateCompletedDto subtaskUpdateDto)
        {
            try
            {
                var task = await _repo.GetTaskByIdAsync(taskId);
                if (task == null)
                {
                    return NotFound();
                }
                var existingSubtask = _repo.GetSubtaskById(task, subtaskId);
                if (existingSubtask == null)
                {
                    return NotFound();
                }
                existingSubtask.IsComplete = subtaskUpdateDto.IsComplete;
                _repo.UpdateTask(task);
                await _repo.SaveChangesAsync();
                return NoContent();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch("{subtaskId}/description")]
        public async Task<IActionResult> UpdateSubtaskDescription(string taskId, string subtaskId, SubtaskUpdateDescriptionDto subtaskUpdateDto)
        {
            try
            {
                var task = await _repo.GetTaskByIdAsync(taskId);
                if (task == null)
                {
                    return NotFound();
                }
                var existingSubtask = _repo.GetSubtaskById(task, subtaskId);
                if (existingSubtask == null)
                {
                    return NotFound();
                }
                existingSubtask.Description = subtaskUpdateDto.Description;
                _repo.UpdateTask(task);
                await _repo.SaveChangesAsync();
                return NoContent();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch("{subtaskId}/order")]
        public async Task<IActionResult> UpdateSubtaskOrder(string taskId, string subtaskId, SubtaskUpdateOrderDto subtaskUpdateDto)
        {
            try
            {
                var task = await _repo.GetTaskByIdAsync(taskId);
                if (task == null)
                {
                    return NotFound();
                }
                var existingSubtask = _repo.GetSubtaskById(task, subtaskId);
                if (existingSubtask == null)
                {
                    return NotFound();
                }
                if (existingSubtask.Order > subtaskUpdateDto.Order)
                {
                    _repo.UpdateSubtaskOrderPush(task, subtaskUpdateDto.Order);
                } else
                {
                    _repo.UpdateSubtaskOrderPull(task, subtaskUpdateDto.Order);
                }
                await _repo.SaveChangesAsync();
                existingSubtask.Order = subtaskUpdateDto.Order;
                _repo.UpdateTask(task);
                await _repo.SaveChangesAsync();
                _repo.ReorderSubtasks(task);
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
