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
        private readonly IDbContextFactory<CosmosContext> _contextFactory;
        private readonly ICurrentUserService _currentUser;

        public ProjectController(IDbContextFactory<CosmosContext> contextFactory, ICurrentUserService currentUser)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
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

            await using var ctx = _contextFactory.CreateDbContext();
            var projects = await ctx.Projects
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

            await using var ctx = _contextFactory.CreateDbContext();
            var project = await ctx.Projects
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

            await using var ctx = _contextFactory.CreateDbContext();
            ctx.Projects.Add(project);
            await ctx.SaveChangesAsync();

            var dto = new Project(project.Id, project.Name, project.Description);
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            var ownerId = GetUserId();
            if (string.IsNullOrEmpty(ownerId)) return Unauthorized(new { error = "Unauthorized" });

            await using var ctx = _contextFactory.CreateDbContext();
            var project = await ctx.Projects.SingleOrDefaultAsync(p => p.Id == id && p.OwnerId == ownerId);
            if (project == null) return NotFound();

            // Optionally delete tasks that belong to this project and owner to avoid orphans
            var tasksToRemove = await ctx.Tasks
                .Where(t => t.ProjectId == project.Id && t.OwnerId == ownerId)
                .ToListAsync();

            if (tasksToRemove.Any())
            {
                ctx.Tasks.RemoveRange(tasksToRemove);
            }

            ctx.Projects.Remove(project);
            await ctx.SaveChangesAsync();

            return NoContent();
        }
    }
}