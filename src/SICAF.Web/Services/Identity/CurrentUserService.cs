using System.Security.Claims;

using SICAF.Business.Interfaces.Identity;
using SICAF.Common.DTOs.Identity;
using SICAF.Web.Interfaces.Identity;

namespace SICAF.Web.Services.Identity;

public class CurrentUserService(
    IHttpContextAccessor httpContextAccessor,
    IUserService userService) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IUserService _userService = userService;

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
            return null;

        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
            return null;

        var result = await _userService.GetUserByIdAsync(userId);
        return result.IsSuccess ? result.Value : null;
    }
}