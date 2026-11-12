using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Recruiting.Interfaces;
using Recruiting.Models;
using Recruiting.Shared.DTOs;
using Recruiting.Shared.Enums;

namespace Recruiting.Services;

public class AuthService : IAuthService
{
    private readonly DataBaseContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICurrentUserService _currentUser;

    public AuthService(
        DataBaseContext db,
        IHttpContextAccessor httpContextAccessor,
        ICurrentUserService currentUser)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
        _currentUser = currentUser;
    }

    public async Task<(bool Success, string? Error, UserDTO? User)> RegisterAsync(RegisterRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var normalizedUserName = request.UserName.Trim();

        if (await _db.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail))
            return (false, "Пользователь с таким Email уже зарегистрирован", null);

        if (await _db.Users.AnyAsync(u => u.UserName.ToLower() == normalizedUserName.ToLower()))
            return (false, "Имя пользователя уже занято", null);

        var user = new User
        {
            Email = normalizedEmail,
            UserName = normalizedUserName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            RoleId = (int)RoleEnum.Hr, // Все новые регистрации — HR
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // Загружаем пользователя с ролью для создания сессии
        var role = await _db.Roles.FindAsync(user.RoleId);
        var roleName = role?.Name ?? "HR";

        await SignInUserAsync(user.Id, user.UserName, user.Email, roleName);

        return (true, null, new UserDTO(user.Id, user.Email, user.UserName, roleName));
    }

    public async Task<(bool Success, string? Error, UserDTO? User)> LoginAsync(LoginRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await _db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return (false, "Неверный email или пароль", null);

        if (!user.IsActive)
            return (false, "Аккаунт заблокирован", null);

        await SignInUserAsync(user.Id, user.UserName, user.Email, user.Role.Name);

        return (true, null, new UserDTO(user.Id, user.Email, user.UserName, user.Role.Name));
    }

    public async Task LogoutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }

    public async Task<UserProfileDTO?> GetCurrentUserProfileAsync()
    {
        if (!_currentUser.UserId.HasValue)
            return null;

        var user = await _db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == _currentUser.UserId.Value);

        if (user == null)
            return null;

        return new UserProfileDTO(
            user.Id,
            user.Email,
            user.UserName,
            user.Role.Name,
            user.IsActive,
            user.CreatedAt
        );
    }

    private async Task SignInUserAsync(int userId, string userName, string email, string roleName)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, userName),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, roleName)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(14)
        };

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);
        }
    }
}