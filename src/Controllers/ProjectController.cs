using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_backend.DbContexts;
using web_backend.Entities;
using web_backend.Services;

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

        // DTOs kept local for brevity
        public record ProjectDto(string Id, string Name, string? Description, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
        public class ProjectForCreationDto
        {
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; }
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

            var dtos = projects.Select(p => new ProjectDto(p.Id, p.Name, p.Description, p.CreatedAt, p.UpdatedAt));
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

            var dto = new ProjectDto(project.Id, project.Name, project.Description, project.CreatedAt, project.UpdatedAt);
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
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                OwnerId = ownerId,
                CreatedAt = now,
                UpdatedAt = now
            };

            await using var ctx = _contextFactory.CreateDbContext();
            ctx.Projects.Add(project);
            await ctx.SaveChangesAsync();

            var dto = new ProjectDto(project.Id, project.Name, project.Description, project.CreatedAt, project.UpdatedAt);
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