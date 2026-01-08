using EnterpriseTaskManager.API.Models.Entities;

namespace EnterpriseTaskManager.API.Models.DTOs.Responses;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string? Department { get; set; }
    public string? Avatar { get; set; }
    public DateTime CreatedAt { get; set; }
}
