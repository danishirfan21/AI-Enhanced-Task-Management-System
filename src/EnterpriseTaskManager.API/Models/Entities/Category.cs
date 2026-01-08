using System.ComponentModel.DataAnnotations;

namespace EnterpriseTaskManager.API.Models.Entities;

/// <summary>
/// Category for organizing tasks
/// </summary>
public class Category
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(7)] // Hex color code like #FF5733
    public string Color { get; set; } = "#6B7280";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
