using Recruiting.Interfaces;
using System.Security.Claims;

namespace Recruiting.Services
{
    public class CurrentUserService: ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
        public int? UserId
        {
            get
            {
                var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(idClaim, out var id) ? id : null;
            }
        }
        public string? UserName => User?.Identity?.Name;
        public string? Role => User?.FindFirst(ClaimTypes.Role)?.Value;
        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
        public bool IsAdmin => Role == "Admin";
    }
}
