using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterpriseTaskManager.API.Models.Entities;

/// <summary>
/// Task/Ticket entity with AI-enhanced features
/// </summary>
public class TaskItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public TaskStatus Status { get; set; } = TaskStatus.Todo;

    [Required]
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public Guid? AssignedToId { get; set; }

    [Required]
    public Guid CreatedById { get; set; }

    public Guid? CategoryId { get; set; }

    /// <summary>
    /// Tags stored as JSON array
    /// </summary>
    [Column(TypeName = "jsonb")]
    public List<string> Tags { get; set; } = new List<string>();

    [Column(TypeName = "decimal(18,2)")]
    public decimal? EstimatedHours { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ActualHours { get; set; }

    public string? CustomerFeedback { get; set; }

    /// <summary>
    /// AI-generated sentiment score from customer feedback
    /// </summary>
    public SentimentType? SentimentScore { get; set; }

    /// <summary>
    /// AI-generated summary of the task and comments
    /// </summary>
    public string? AISummary { get; set; }

    /// <summary>
    /// Timestamp when AI summary was last generated
    /// </summary>
    public DateTime? AISummaryGeneratedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }

    // Navigation properties
    [ForeignKey(nameof(AssignedToId))]
    public virtual User? AssignedTo { get; set; }

    [ForeignKey(nameof(CreatedById))]
    public virtual User CreatedBy { get; set; } = null!;

    [ForeignKey(nameof(CategoryId))]
    public virtual Category? Category { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
