using Microsoft.EntityFrameworkCore;
using web_backend.DbContexts;
using web_backend.Entities;
using web_backend.Models;

namespace web_backend.Services
{
    public class TaskEntityRepository : ITaskEntityRepository
    {
        private readonly CosmosContext _context;
        private readonly ICurrentUserService _currentUser;
        public TaskEntityRepository(CosmosContext context, ICurrentUserService currentUser)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            //_context.Database.EnsureDeleted();
            //_context.Database.EnsureCreated();
        }

        // TaskEntity methods (current-user scoped internally)

        public async Task<IEnumerable<TaskEntity>> GetTasksAsync()
        {
            var ownerId = _currentUser.UserId;
            if (string.IsNullOrEmpty(ownerId)) return Enumerable.Empty<TaskEntity>();

            return await _context.Tasks
                .Where(t => t.OwnerId == ownerId)
                .OrderBy(t => t.Order)
                .ToListAsync();
        }

        public async Task<TaskEntity?> GetTaskByIdAsync(string taskId)
        {
            try
            {
                var ownerId = _currentUser.UserId;
                if (string.IsNullOrEmpty(ownerId)) return null;

                return await _context.Tasks
                    .SingleOrDefaultAsync(t => t.Id == taskId && t.OwnerId == ownerId);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public void ReorderTasks(string projectId)
        {
            var ownerId = _currentUser.UserId;
            if (string.IsNullOrEmpty(ownerId)) return;

            var orderedTasks = _context.Tasks
                .Where(t => t.OwnerId == ownerId && t.ProjectId == projectId)
                .OrderBy(t => t.Order)
                .ToList();
            for (int i = 0; i < orderedTasks.Count; i++)
            {
                orderedTasks[i].Order = i + 1;
            }
        }

        public void AddTask(TaskEntity task)
        {
            // ensure OwnerId is set to current user
            var ownerId = _currentUser.UserId;
            if (!string.IsNullOrEmpty(ownerId) && string.IsNullOrEmpty(task.OwnerId))
            {
                task.OwnerId = ownerId;
            }

            _context.Tasks.Add(task);
        }

        public void RemoveTask(TaskEntity task)
        {
            _context.Tasks.Remove(task);
        }

        public void UpdateTask(TaskEntity task)
        {
            _context.Tasks.Update(task);
        }

        public void UpdateTaskOrderPull(int newOrder)
        {
            var ownerId = _currentUser.UserId;
            if (string.IsNullOrEmpty(ownerId)) return;

            var affectedTasks = _context.Tasks
                .Where(t => t.OwnerId == ownerId && t.Order <= newOrder)
                .OrderBy(t => t.Order)
                .ToList();

            foreach (var affectedTask in affectedTasks)
            {
                affectedTask.Order--;
            }
        }

        public void UpdateTaskOrderPush(int newOrder)
        {
            var ownerId = _currentUser.UserId;
            if (string.IsNullOrEmpty(ownerId)) return;

            var affectedTasks = _context.Tasks
                .Where(t => t.OwnerId == ownerId && t.Order >= newOrder)
                .OrderBy(t => t.Order)
                .ToList();

            foreach (var affectedTask in affectedTasks)
            {
                affectedTask.Order++;
            }
        }

        // Subtask methods

        public void ReorderSubtasks(TaskEntity task)
        {
            var orderedSubtasks = task.Subtasks.OrderBy(t => t.Order).ToList();
            for (int i = 0; i < orderedSubtasks.Count(); i++)
            {
                orderedSubtasks[i].Order = i + 1;
            }
        }
        public void UpdateSubtaskOrderPush(TaskEntity task, int newOrder)
        {
            var affectedSubtasks = task.Subtasks
                .Where(t => t.Order >= newOrder)
                .OrderBy(t => t.Order)
                .ToList();

            foreach (var affectedSubtask in affectedSubtasks)
            {
                affectedSubtask.Order++;
            }
        }

        public void UpdateSubtaskOrderPull(TaskEntity task, int newOrder)
        {
            var affectedSubtasks = task.Subtasks
                .Where(t => t.Order <= newOrder)
                .OrderBy(t => t.Order)
                .ToList();

            foreach (var affectedSubtask in affectedSubtasks)
            {
                affectedSubtask.Order--;
            }
        }
        public IEnumerable<Subtask> GetSubtasks(TaskEntity task)
        {
            return task.Subtasks.OrderBy(s => s.Order);
        }

        public Subtask? GetSubtaskById(TaskEntity task, string subtaskId)
        {
            try
            {
                return task.Subtasks.FirstOrDefault(t => t.Id == subtaskId);
            }            
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }  
        }

        public void AddSubtask(TaskEntity task, Subtask subtask)
        {
            task.Subtasks.Add(subtask);
        }

        public void RemoveSubtask(TaskEntity task, string subtaskId)
        {
            Subtask? subtask = task.Subtasks.FirstOrDefault(t => t.Id == subtaskId);
            if (subtask != null) task.Subtasks.Remove(subtask);
        }

        public void UpdateSubtask(Subtask subtask)
        {
            throw new NotImplementedException();
        }

        // Step methods

        public IEnumerable<Step> GetSteps(Subtask subtask)
        {
            return subtask.Steps;
        }

        public Step? GetStepById(Subtask subtask,  string stepId)
        {
            try
            {
                return subtask.Steps.FirstOrDefault(t => t.Id == stepId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public void UpdateStepOrderPush(Subtask subtask, int newOrder)
        {
            var affectedSteps = subtask.Steps
                .Where(t => t.Order >= newOrder)
                .OrderBy(t => t.Order)
                .ToList();

            foreach (var affectedStep in affectedSteps)
            {
                affectedStep.Order++;
            }
        }

        public void UpdateStepOrderPull(Subtask subtask, int newOrder)
        {
            var affectedSteps = subtask.Steps
                .Where(t => t.Order <= newOrder)
                .OrderBy(t => t.Order)
                .ToList();

            foreach (var affectedStep in affectedSteps)
            {
                affectedStep.Order--;
            }
        }

        public void ReorderSteps(Subtask subtask)
        {
            var orderedSteps = subtask.Steps.OrderBy(t => t.Order).ToList();
            for (int i = 0; i < orderedSteps.Count(); i++)
            {
                orderedSteps[i].Order = i + 1;
            }
        }

        public void AddStep(Subtask subtask, Step step)
        {
            subtask.Steps.Add(step);
        }

        public void RemoveStep(Subtask subtask, string stepId)
        {
            Step? step = subtask.Steps.FirstOrDefault(t => t.Id == stepId);
            if (step != null)
                subtask.Steps.Remove(step);
        }

        public void UpdateStep(Step step)
        {
            throw new NotImplementedException();
        }

        // Save changes
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

    }
}
