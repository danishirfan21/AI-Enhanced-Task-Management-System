using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterpriseTaskManager.API.Models.Entities;

/// <summary>
/// Log of AI feature usage for analytics and cost tracking
/// </summary>
public class AIUsageLog
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string FeatureType { get; set; } = string.Empty; // Summary, Autofill, Search, Sentiment, Assignment

    public string? Prompt { get; set; }

    public string? Response { get; set; }

    public int? TokensUsed { get; set; }

    /// <summary>
    /// Execution time in milliseconds
    /// </summary>
    public int ExecutionTimeMs { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string Provider { get; set; } = "Ollama"; // Ollama or OpenAI

    public bool Success { get; set; } = true;

    public string? ErrorMessage { get; set; }

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
}
