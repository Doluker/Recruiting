using Recruiting.Shared.DTOs;

namespace Recruiting.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string? Error, UserDTO? User)> RegisterAsync(RegisterRequest request);
        Task<(bool Success, string? Error, UserDTO? User)> LoginAsync(LoginRequest request);
        Task LogoutAsync();
        Task<UserProfileDTO?> GetCurrentUserProfileAsync();
    }
}
