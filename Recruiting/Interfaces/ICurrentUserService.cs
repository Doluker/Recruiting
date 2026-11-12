namespace Recruiting.Interfaces
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? UserName { get; }
        string? Role { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
    }
}
