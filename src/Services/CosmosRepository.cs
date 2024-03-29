﻿using Microsoft.EntityFrameworkCore;
using web_backend.DbContexts;
using web_backend.Entities;

namespace web_backend.Services
{
    public class CosmosRepository : IRepository
    {
        private readonly CosmosContext _context;

        public CosmosRepository(CosmosContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //_context.Database.EnsureDeleted();
            //_context.Database.EnsureCreated();
        }

        // Comment methods

         public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }
        public async Task<Comment> GetCommentByIdAsync(string commentId)
        {
            try
            {
                Comment comment = await _context.Comments.SingleOrDefaultAsync(c => c.Id == commentId);
                return comment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public void RemoveComment(Comment comment)
        {
            _context.Comments.Remove(comment);
        }


        // TaskEntity methods

        public async Task<IEnumerable<TaskEntity>> GetTasksAsync()
        {
            return await _context.Tasks.OrderBy(t => t.Order).ToListAsync();
        }

        public async Task<TaskEntity> GetTaskByIdAsync(string taskId)
        {
            try
            {
                TaskEntity task = await _context.Tasks.SingleOrDefaultAsync(t => t.Id == taskId);
                return task;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public void ReorderTasks()
        {
            var orderedTasks = _context.Tasks.OrderBy(t => t.Order).ToList();
            for (int i = 0; i < orderedTasks.Count; i++)
            {
                orderedTasks[i].Order = i+1;
            }
        }

        public void AddTask(TaskEntity task)
        {
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
            var affectedTasks = _context.Tasks
                .Where(t => t.Order <= newOrder)
                .OrderBy(t => t.Order)
                .ToList();

            foreach (var affectedTask in affectedTasks)
            {
                affectedTask.Order--;
            }
        }

        public void UpdateTaskOrderPush(int newOrder)
        {
            var affectedTasks = _context.Tasks
                .Where(t => t.Order >= newOrder)
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

        public Subtask GetSubtaskById(TaskEntity task, string subtaskId)
        {
            Subtask subtask = task.Subtasks.FirstOrDefault(t => t.Id == subtaskId);
            if (subtask == null) 
            {
                return null;
            }
            return subtask;
        }

        public void AddSubtask(TaskEntity task, Subtask subtask)
        {
            task.Subtasks.Add(subtask);
        }

        public void RemoveSubtask(TaskEntity task, string subtaskId)
        {
            Subtask subtask = task.Subtasks.FirstOrDefault(t => t.Id == subtaskId);
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

        public Step GetStepById(Subtask subtask,  string stepId)
        {
            Step step = subtask.Steps.FirstOrDefault(t => t.Id == stepId);
            if (step == null)
            {
                return null;
            }
            return step;
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
            Step step = GetStepById(subtask, stepId);
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
