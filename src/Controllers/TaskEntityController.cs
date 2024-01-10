﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Collections.Specialized;
using web_backend.Entities;
using web_backend.Models;
using web_backend.Services;

namespace web_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskEntityController : Controller
    {
        private readonly IRepository _repo;
        private readonly NameValueCollection settings =
            System.Configuration.ConfigurationManager.AppSettings;
        public TaskEntityController(IRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(string id)
        {
            var task = await _repo.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            TaskEntityDto taskDtoToReturn = new TaskEntityDto(task.Id, task.Timestamp, task.Description, task.IsComplete, task.Subtasks);
            return Ok(taskDtoToReturn);
        }

        [HttpGet(Name = "GetTasks")]
        public async Task<IEnumerable<TaskEntityDto>> GetAllTasks()
        {
            var tasksFromDb = await _repo.GetTasksAsync();
            var taskDtosToReturn = new List<TaskEntityDto>();
            foreach (var task in tasksFromDb)
            {
                taskDtosToReturn.Add(new TaskEntityDto(task.Id, task.Timestamp, task.Description, task.IsComplete, task.Subtasks));
            }
            return taskDtosToReturn;
        }

        [HttpPost]
        public async Task<ActionResult<TaskEntityDto>> createTask(
            TaskEntityForCreationDto task)
        {
            var taskEntity = new TaskEntity(task.Description, false);
            _repo.AddTask(taskEntity);
            await _repo.SaveChangesAsync();
            var taskToReturn = new TaskEntityDto(taskEntity.Id, taskEntity.Timestamp, taskEntity.Description, taskEntity.IsComplete, taskEntity.Subtasks);
            return CreatedAtRoute("GetTasks", taskToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteTask(
            string id)
        {
            var task = await _repo.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            _repo.RemoveTask(task);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<TaskEntityDto>> UpdateTaskCompleted(string id, TaskEntityUpdateCompletedDto taskUpdateDto)
        {
            var existingTask = await _repo.GetTaskByIdAsync(id);

            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.IsComplete = taskUpdateDto.IsComplete;
            
            _repo.UpdateTask(existingTask);
            await _repo.SaveChangesAsync();

            var updatedTask = new TaskEntityDto(existingTask.Id, existingTask.Timestamp, existingTask.Description, existingTask.IsComplete, existingTask.Subtasks);
            return Ok(updatedTask);
        }
    }
}
