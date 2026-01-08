using System.ComponentModel.DataAnnotations;
using EnterpriseTaskManager.API.Models.Entities;
using TaskStatus = EnterpriseTaskManager.API.Models.Entities.TaskStatus;

namespace EnterpriseTaskManager.API.Models.DTOs.Requests;

public class CreateTaskRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public TaskStatus Status { get; set; } = TaskStatus.Todo;

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public Guid? AssignedToId { get; set; }

    public Guid? CategoryId { get; set; }

    public List<string> Tags { get; set; } = new();

    public decimal? EstimatedHours { get; set; }

    public DateTime? DueDate { get; set; }

    public string? CustomerFeedback { get; set; }
}
