using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_backend.DbContexts;
using web_backend.Entities;
using web_backend.Services;
using web_backend.Models.DTOs;

namespace web_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize] // controller requires authenticated user (global fallback policy exists too)
    public class ProjectController : ControllerBase
    {
        private readonly CosmosContext _context;
        private readonly ICurrentUserService _currentUser;

        public ProjectController(CosmosContext context, ICurrentUserService currentUser)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        private string GetUserId()
        {
            return _currentUser.UserId ?? string.Empty;
        }

        [HttpGet(Name = "GetProjects")]
        public async Task<IActionResult> GetProjects()
        {
            var ownerId = GetUserId();
            if (string.IsNullOrEmpty(ownerId)) return Unauthorized(new { error = "Unauthorized" });

            var projects = await _context.Projects
                .Where(p => p.OwnerId == ownerId)
                .OrderBy(p => p.Name)
                .ToListAsync();

            var dtos = projects.Select(p => new ProjectDto(p.Id, p.Name, p.Description, p.CreatedAt));
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(string id)
        {
            var ownerId = GetUserId();
            if (string.IsNullOrEmpty(ownerId)) return Unauthorized(new { error = "Unauthorized" });

            var project = await _context.Projects
                .SingleOrDefaultAsync(p => p.Id == id && p.OwnerId == ownerId);

            if (project == null) return NotFound();

            var dto = new ProjectDto(project.Id, project.Name, project.Description, project.CreatedAt);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] ProjectForCreationDto request)
        {
            if (request == null) return BadRequest();

            var ownerId = GetUserId();
            if (string.IsNullOrEmpty(ownerId)) return Unauthorized(new { error = "Unauthorized" });

            var now = DateTimeOffset.UtcNow;

            // Create a Project entity and set OwnerId explicitly
            var project = new Project
            (   
                request.Name,
                request.Description,
                ownerId
            );

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var dto = new ProjectDto(project.Id, project.Name, project.Description, project.CreatedAt);
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            var ownerId = GetUserId();
            if (string.IsNullOrEmpty(ownerId)) return Unauthorized(new { error = "Unauthorized" });

            var project = await _context.Projects.SingleOrDefaultAsync(p => p.Id == id && p.OwnerId == ownerId);
            if (project == null) return NotFound();

            // Optionally delete tasks that belong to this project and owner to avoid orphans
            var tasksToRemove = await _context.Tasks
                .Where(t => t.ProjectId == project.Id && t.OwnerId == ownerId)
                .ToListAsync();

            if (tasksToRemove.Any())
            {
                _context.Tasks.RemoveRange(tasksToRemove);
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}