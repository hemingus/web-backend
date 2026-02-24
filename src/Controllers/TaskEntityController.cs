using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Collections.Specialized;
using web_backend.Entities;
using web_backend.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using web_backend.Models.DTOs;

namespace web_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize] // Require authenticated users
    public class TaskEntityController : Controller
    {
        private readonly ITaskEntityRepository _repo;
        private readonly NameValueCollection settings =
            System.Configuration.ConfigurationManager.AppSettings;
        public TaskEntityController(ITaskEntityRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        private string GetUserId()
        {
            // Try common claim types (sub or NameIdentifier)
            var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(sub)) return sub;
            return User.FindFirst("sub")?.Value ?? string.Empty;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(string id)
        {
            try
            {
                var ownerId = GetUserId();
                var task = await _repo.GetTaskByIdAsync(id, ownerId);
                if (task == null) return NotFound();
                TaskEntityDto taskDtoToReturn = new TaskEntityDto(task.Id, task.Timestamp, task.Description, task.IsComplete, task.Order, task.Subtasks);
                return Ok(taskDtoToReturn);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet(Name = "GetTasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var ownerId = GetUserId();
                var tasksFromDb = await _repo.GetTasksAsync(ownerId);
                var taskDtosToReturn = new List<TaskEntityDto>();
                
                foreach (var task in tasksFromDb)
                {
                    taskDtosToReturn.Add(new TaskEntityDto(task.Id, task.Timestamp, task.Description, task.IsComplete, task.Order, task.Subtasks));
                }
                return Ok(taskDtosToReturn);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TaskEntityDto>> CreateTask(
            TaskEntityForCreationDto task, string projectId)
        {
            try
            {
                var ownerId = GetUserId();
                var taskEntity = new TaskEntity(task.Description, false, task.Order, ownerId, projectId);
                taskEntity.OwnerId = ownerId; // associate with the logged-in user
                _repo.AddTask(taskEntity);
                await _repo.SaveChangesAsync();
                var taskToReturn = new TaskEntityDto(
                    taskEntity.Id, taskEntity.Timestamp, taskEntity.Description,
                    taskEntity.IsComplete, taskEntity.Order, taskEntity.Subtasks);
                return CreatedAtRoute("GetTasks", taskToReturn);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(
            string id)
        {
            try
            {
                var ownerId = GetUserId();
                var task = await _repo.GetTaskByIdAsync(id, ownerId);
                if (task == null) return NotFound();
                _repo.RemoveTask(task);
                await _repo.SaveChangesAsync();
                _repo.ReorderTasks(ownerId);
                await _repo.SaveChangesAsync();
                return NoContent();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<TaskEntityDto>> UpdateTaskCompleted(string id, TaskEntityUpdateCompletedDto taskUpdateDto)
        {
            try
            {
                var ownerId = GetUserId();
                var existingTask = await _repo.GetTaskByIdAsync(id, ownerId);

                if (existingTask == null)
                {
                    return NotFound();
                }

                existingTask.IsComplete = taskUpdateDto.IsComplete;

                _repo.UpdateTask(existingTask);
                await _repo.SaveChangesAsync();

                var updatedTask = new TaskEntityDto(
                    existingTask.Id, existingTask.Timestamp, existingTask.Description, existingTask.IsComplete, 
                    existingTask.Order, existingTask.Subtasks);
                return Ok(updatedTask);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch("{id}/description")]
        public async Task<ActionResult<TaskEntityDto>> UpdateTaskCompleted(string id, TaskEntityUpdateDescriptionDto taskUpdateDto)
        {
            try
            {
                var ownerId = GetUserId();
                var existingTask = await _repo.GetTaskByIdAsync(id, ownerId);

                if (existingTask == null)
                {
                    return NotFound();
                }

                existingTask.Description = taskUpdateDto.Description;

                _repo.UpdateTask(existingTask);
                await _repo.SaveChangesAsync();

                var updatedTask = new TaskEntityDto(
                    existingTask.Id, existingTask.Timestamp, existingTask.Description, 
                    existingTask.IsComplete, existingTask.Order, existingTask.Subtasks);
                return Ok(updatedTask);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPatch("{id}/order")]
        public async Task<ActionResult<TaskEntityDto>> UpdateTaskOrder(string id, TaskEntityUpdateOrderDto taskUpdateDto)
        {
            try
            {
                var ownerId = GetUserId();
                var existingTask = await _repo.GetTaskByIdAsync(id, ownerId);

                if (existingTask == null)
                {
                    return NotFound();
                }
                if (existingTask.Order > taskUpdateDto.Order)
                {
                    _repo.UpdateTaskOrderPush(ownerId, taskUpdateDto.Order);
                }
                else
                {
                    _repo.UpdateTaskOrderPull(ownerId, taskUpdateDto.Order);
                }
                existingTask.Order = taskUpdateDto.Order;
                await _repo.SaveChangesAsync();
                _repo.UpdateTask(existingTask);
                await _repo.SaveChangesAsync();
                _repo.ReorderTasks(ownerId);

                await _repo.SaveChangesAsync();

                var updatedTask = new TaskEntityDto(
                    existingTask.Id, existingTask.Timestamp, existingTask.Description,
                    existingTask.IsComplete, existingTask.Order, existingTask.Subtasks);
                return Ok(updatedTask);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
