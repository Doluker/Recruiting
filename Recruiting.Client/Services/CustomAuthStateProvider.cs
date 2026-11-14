using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Recruiting.Shared.DTOs;

namespace Recruiting.Client.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _http;
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

    public CustomAuthStateProvider(HttpClient http)
    {
        _http = http;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var response = await _http.GetAsync("api/auth/me");
            if (!response.IsSuccessStatusCode)
            {
                return new AuthenticationState(Anonymous);
            }

            var profile = await response.Content.ReadFromJsonAsync<UserProfileDTO>();
            if (profile == null)
            {
                return new AuthenticationState(Anonymous);
            }

            var identity = CreateIdentityFromProfile(profile);
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return new AuthenticationState(Anonymous);
        }
    }

    public void NotifyUserLogin(UserDTO user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.RoleName)
        };
        var identity = new ClaimsIdentity(claims, "CookieAuth");
        var principal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(Anonymous)));
    }

    private static ClaimsIdentity CreateIdentityFromProfile(UserProfileDTO profile)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, profile.Id.ToString()),
            new(ClaimTypes.Name, profile.UserName),
            new(ClaimTypes.Email, profile.Email),
            new(ClaimTypes.Role, profile.RoleName)
        };
        return new ClaimsIdentity(claims, "CookieAuth");
    }
}