namespace Recruiting.Client.Interfaces
{
    public interface IAuthApiClient
    {
        Task<(bool Success, string? Error)> LoginAsync(string email, string password);
        Task<(bool Success, string? Error)> RegisterAsync(string email, string userName, string password);
        Task LogoutAsync();
    }
}
