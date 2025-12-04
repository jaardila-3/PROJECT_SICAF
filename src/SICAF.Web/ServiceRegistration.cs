using System.Text.Json;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

using SICAF.Web.Interfaces.Audit;
using SICAF.Web.Interfaces.Files;
using SICAF.Web.Interfaces.Identity;
using SICAF.Web.Interfaces.Pdf;
using SICAF.Web.Services.Audit;
using SICAF.Web.Services.Files;
using SICAF.Web.Services.Identity;
using SICAF.Web.Services.Pdf;

namespace SICAF.Web;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterWebServices(this IServiceCollection services)
    {
        // Configuration sesion
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        //configuration auth and cookies
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.LogoutPath = "/Identity/Account/Logout";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.Name = "SICAFCookie";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
            });

        // configure the application routes to use lowercase urls
        services.AddRouting(options => options.LowercaseUrls = true);

        // Add services to the container with global authorization
        services.AddControllersWithViews(options =>
        {
            // Add global authorization policy
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        })
        // Configure MVC JSON options
        .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
        ;

        // Add IHttpContextAccessor
        services.AddHttpContextAccessor();

        // Add services
        services.AddScoped<IAuditContext, HttpAuditContext>();
        services.AddScoped<IImageValidationService, ImageValidationService>();
        services.AddScoped<IUserValidationService, UserValidationService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPuppeteerService, PuppeteerService>();
        services.AddScoped<IQuestPDFService, QuestPDFService>();
        services.AddScoped<IChartGeneratorService, ChartGeneratorService>();

        return services;
    }
}