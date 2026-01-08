using EnterpriseTaskManager.API.Models.Entities;

namespace EnterpriseTaskManager.API.Models.DTOs.Responses;

public class TaskResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public UserResponse? AssignedTo { get; set; }
    public UserResponse CreatedBy { get; set; } = null!;
    public CategoryResponse? Category { get; set; }
    public List<string> Tags { get; set; } = new();
    public decimal? EstimatedHours { get; set; }
    public decimal? ActualHours { get; set; }
    public string? CustomerFeedback { get; set; }
    public SentimentType? SentimentScore { get; set; }
    public string? AISummary { get; set; }
    public DateTime? AISummaryGeneratedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public int CommentCount { get; set; }
}

public class CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = string.Empty;
}
