using System.ComponentModel.DataAnnotations;
using EnterpriseTaskManager.API.Models.Entities;

namespace EnterpriseTaskManager.API.Models.DTOs.Requests;

public class RegisterRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Department { get; set; }

    public UserRole Role { get; set; } = UserRole.User;
}
