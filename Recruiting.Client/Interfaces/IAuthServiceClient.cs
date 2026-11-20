using Recruiting.Shared.DTOs;

namespace Recruiting.Client.Interfaces
{
    public interface IAuthServiceClient
    {
        Task<(bool Success, string? Error)> LoginAsync(string email, string password);
        Task<(bool Success, string? Error)> RegisterAsync(string email, string userName, string password);
        Task LogoutAsync();
        Task<List<UserProfileDTO>?> GetAllUsersAsync();
        Task<bool> ChangeUserRoleAsync(int userId, int roleId);
    }
}
