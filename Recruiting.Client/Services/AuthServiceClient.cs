using Microsoft.AspNetCore.Components.Authorization;
using Recruiting.Client.Interfaces;
using Recruiting.Shared.DTOs;
using System.Net.Http.Json;

namespace Recruiting.Client.Services
{
    public class AuthServiceClient:IAuthServiceClient
    {
        private readonly HttpClient _http;
        private readonly CustomAuthStateProvider _authStateProvider;
        public AuthServiceClient(HttpClient http, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _authStateProvider = (CustomAuthStateProvider)authStateProvider;
        }
        public async Task<(bool Success, string? Error)> LoginAsync(string email, string password)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/auth/login", new LoginRequest(email, password));
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserDTO>();
                    if (user != null)
                    {
                        _authStateProvider.NotifyUserLogin(user);
                        return (true, null);
                    }
                }
                var error = await response.Content.ReadFromJsonAsync<ApiError>();
                return (false, error?.Message ?? "Неверный логин или пароль");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task<(bool Success, string? Error)> RegisterAsync(string email, string userName, string password)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/auth/register", new RegisterRequest(email, userName, password));
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserDTO>();
                    if (user != null)
                    {
                        _authStateProvider.NotifyUserLogin(user);
                        return (true, null);
                    }
                }
                var error = await response.Content.ReadFromJsonAsync<ApiError>();
                return (false, error?.Message ?? "Ошибка регистрации");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task LogoutAsync()
        {
            try
            {
                await _http.PostAsync("api/auth/logout", null);
            }
            finally
            {
                _authStateProvider.NotifyUserLogout();
            }
        }
        public async Task<List<UserProfileDTO>?> GetAllUsersAsync()
        {
            return await _http.GetFromJsonAsync<List<UserProfileDTO>>("api/auth/users");
        }

        public async Task<bool> ChangeUserRoleAsync(int userId, int roleId)
        {
            var response = await _http.PutAsync($"api/auth/users/{userId}/role?roleId={roleId}", null);
            return response.IsSuccessStatusCode;
        }
    }
}
