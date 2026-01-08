using Microsoft.EntityFrameworkCore;
using EnterpriseTaskManager.API.Data;
using EnterpriseTaskManager.API.Models.DTOs.Requests;
using EnterpriseTaskManager.API.Models.DTOs.Responses;
using EnterpriseTaskManager.API.Models.Entities;

namespace EnterpriseTaskManager.API.Services;

public class TaskService
{
    private readonly AppDbContext _context;
    private readonly ILogger<TaskService> _logger;

    public TaskService(AppDbContext context, ILogger<TaskService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<TaskResponse>> GetTasksAsync()
    {
        var tasks = await _context.Tasks
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .Include(t => t.Category)
            .Include(t => t.Comments)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return tasks.Select(MapToResponse).ToList();
    }

    public async Task<TaskResponse?> GetTaskByIdAsync(Guid id)
    {
        var task = await _context.Tasks
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .Include(t => t.Category)
            .Include(t => t.Comments)
            .FirstOrDefaultAsync(t => t.Id == id);

        return task != null ? MapToResponse(task) : null;
    }

    public async Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request, Guid userId)
    {
        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            Priority = request.Priority,
            AssignedToId = request.AssignedToId,
            CreatedById = userId,
            CategoryId = request.CategoryId,
            Tags = request.Tags,
            EstimatedHours = request.EstimatedHours,
            DueDate = request.DueDate,
            CustomerFeedback = request.CustomerFeedback,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Task created: {TaskId} by User: {UserId}", task.Id, userId);

        // Reload with navigation properties
        return (await GetTaskByIdAsync(task.Id))!;
    }

    public async Task<TaskResponse?> UpdateTaskAsync(Guid id, CreateTaskRequest request, Guid userId)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return null;

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.Priority = request.Priority;
        task.AssignedToId = request.AssignedToId;
        task.CategoryId = request.CategoryId;
        task.Tags = request.Tags;
        task.EstimatedHours = request.EstimatedHours;
        task.DueDate = request.DueDate;
        task.CustomerFeedback = request.CustomerFeedback;
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Task updated: {TaskId} by User: {UserId}", id, userId);

        return await GetTaskByIdAsync(id);
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Task deleted: {TaskId}", id);

        return true;
    }

    public async Task<TaskResponse?> GenerateSummaryAsync(Guid taskId)
    {
        var task = await _context.Tasks
            .Include(t => t.Comments)
            .FirstOrDefaultAsync(t => t.Id == taskId);

        if (task == null) return null;

        // For MVP, we'll set a placeholder. The AI service will populate this.
        task.AISummary = "AI Summary will be generated here";
        task.AISummaryGeneratedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetTaskByIdAsync(taskId);
    }

    private TaskResponse MapToResponse(TaskItem task)
    {
        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            AssignedTo = task.AssignedTo != null ? new UserResponse
            {
                Id = task.AssignedTo.Id,
                Name = task.AssignedTo.Name,
                Email = task.AssignedTo.Email,
                Role = task.AssignedTo.Role,
                Department = task.AssignedTo.Department,
                Avatar = task.AssignedTo.Avatar,
                CreatedAt = task.AssignedTo.CreatedAt
            } : null,
            CreatedBy = new UserResponse
            {
                Id = task.CreatedBy.Id,
                Name = task.CreatedBy.Name,
                Email = task.CreatedBy.Email,
                Role = task.CreatedBy.Role,
                Department = task.CreatedBy.Department,
                Avatar = task.CreatedBy.Avatar,
                CreatedAt = task.CreatedBy.CreatedAt
            },
            Category = task.Category != null ? new CategoryResponse
            {
                Id = task.Category.Id,
                Name = task.Category.Name,
                Description = task.Category.Description,
                Color = task.Category.Color
            } : null,
            Tags = task.Tags,
            EstimatedHours = task.EstimatedHours,
            ActualHours = task.ActualHours,
            CustomerFeedback = task.CustomerFeedback,
            SentimentScore = task.SentimentScore,
            AISummary = task.AISummary,
            AISummaryGeneratedAt = task.AISummaryGeneratedAt,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            DueDate = task.DueDate,
            CommentCount = task.Comments?.Count ?? 0
        };
    }
}
