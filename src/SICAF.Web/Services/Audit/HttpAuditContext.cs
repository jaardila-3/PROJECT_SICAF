using System.Security.Claims;

using SICAF.Common.Constants;
using SICAF.Web.Interfaces.Audit;

namespace SICAF.Web.Services.Audit;

public class HttpAuditContext(IHttpContextAccessor httpContextAccessor) : IAuditContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid LoggedUserId => GetUserId();
    public string UserName => GetUserName();
    public string IdentificationNumber => GetIdentificationNumber();
    public string Grade => GetGrade();
    public string WingType => GetWingType();
    public string Name => GetName();
    public string LastName => GetLastName();
    public string? UserRole => GetUserRole();
    public string? IpAddress => GetIpAddress();
    public string? HttpMethod => GetHttpMethod();
    public string? Url => GetUrl();
    public string? Module => GetModule();
    public string? Controller => GetController();
    public string? Action => GetAction();

    #region Private Methods
    private Guid GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId != null ? Guid.Parse(userId) : Guid.Empty;
    }

    private string GetUserName()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.Identity?.Name ?? string.Empty;
    }

    private string GetName()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty;
        // other option: user?.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;
    }

    private string GetLastName()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty;
    }

    private string GetGrade()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.FindFirst(CustomClaimTypes.Grade)?.Value ?? string.Empty;
    }

    private string GetIdentificationNumber()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.FindFirst(CustomClaimTypes.IdentificationNumber)?.Value ?? string.Empty;
    }

    private string GetWingType()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.FindFirst(CustomClaimTypes.WingType)?.Value ?? string.Empty;
    }

    private string GetUserRole()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var roles = user?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        return roles?.Count > 0 ? string.Join(",", roles) : string.Empty;
    }

    private string? GetIpAddress()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return null;

        // Intentar obtener la IP real si está detrás de un proxy
        var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwarded))
            return forwarded.Split(',').First().Trim();

        var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
            return realIp;

        return context.Connection.RemoteIpAddress?.ToString();
    }
    private string? GetHttpMethod()
    {
        var context = _httpContextAccessor.HttpContext;
        return context?.Request.Method;
    }

    private string? GetUrl()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return null;

        return $"{context.Request.Path}{context.Request.QueryString}";
    }

    private string? GetModule()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return null;

        var area = context.GetRouteValue("area")?.ToString();
        return area;
    }

    private string? GetController()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return null;

        var controller = context.GetRouteValue("controller")?.ToString();
        return controller;
    }

    private string? GetAction()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return null;

        var action = context.GetRouteValue("action")?.ToString();
        return action;
    }
    #endregion
}