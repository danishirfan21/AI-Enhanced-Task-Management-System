using EnterpriseTaskManager.API.Models.DTOs.Requests;
using EnterpriseTaskManager.API.Models.DTOs.Responses;

namespace EnterpriseTaskManager.API.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    Task<bool> RevokeTokenAsync(string refreshToken);
}
