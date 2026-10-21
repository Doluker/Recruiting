
namespace Recruiting.Shared.DTOs
{
    public record LoginRequest(string Email, string Password);
    public record RegisterRequest(string Email, string UserName, string Password);
    public record AuthResponse(string Token, DateTime ExpiresAt, UserDto User);
    public record UserDto(int Id, string Email, string UserName, string RoleName);
    public record ChangePasswordRequest(string OldPassword, string NewPassword);
    public record UserProfileDto(int Id, string Email, string UserName, string RoleName, bool IsActive, DateTime CreatedAt);
}
