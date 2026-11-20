using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recruiting.Interfaces;
using Recruiting.Shared.DTOs;

namespace Recruiting.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var (success, error, user) = await _authService.RegisterAsync(request);
        if (!success)
        {
            return BadRequest(new ApiError("REGISTRATION_FAILED", error ?? "Ошибка регистрации"));
        }

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (success, error, user) = await _authService.LoginAsync(request);
        if (!success)
        {
            return BadRequest(new ApiError("LOGIN_FAILED", error ?? "Неверный логин или пароль"));
        }

        return Ok(user);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return Ok(new OperationResult(true, "Успешный выход из системы"));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var profile = await _authService.GetCurrentUserProfileAsync();
        if (profile == null)
        {
            return Unauthorized(new ApiError("UNAUTHORIZED", "Сессия не найдена"));
        }

        return Ok(profile);
    }
    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers([FromServices] Recruiting.Models.DataBaseContext db)
    {
        var users = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
            db.Users.Include(u => u.Role).Select(u => new UserProfileDTO(
                u.Id,
                u.Email,
                u.UserName,
                u.Role.Name,
                u.IsActive,
                u.CreatedAt
            ))
        );
        return Ok(users);
    }

    [HttpPut("users/{id:int}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeUserRole(int id, [FromQuery] int roleId, [FromServices] Recruiting.Models.DataBaseContext db)
    {
        var user = await db.Users.FindAsync(id);
        if (user == null) return NotFound(new ApiError("NOT_FOUND", "Пользователь не найден"));

        user.RoleId = roleId; // 1 = Admin, 2 = Hr
        await db.SaveChangesAsync();
        return Ok(new OperationResult(true, "Роль пользователя обновлена"));
    }
}