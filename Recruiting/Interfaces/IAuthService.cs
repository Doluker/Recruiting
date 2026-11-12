using Recruiting.Shared.DTOs;

namespace Recruiting.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string? Error, UserDto? User)> RegisterAsync(RegisterRequest request);
        Task<(bool Success, string? Error, UserDto? User)> LoginAsync(LoginRequest request);
        Task LogoutAsync();
        Task<UserProfileDto?> GetCurrentUserProfileAsync();
    }
}
