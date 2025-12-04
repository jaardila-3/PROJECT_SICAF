using Microsoft.Extensions.DependencyInjection;

using SICAF.Business.Interfaces.Academic;
using SICAF.Business.Interfaces.Auditing;
using SICAF.Business.Interfaces.Catalogs;
using SICAF.Business.Interfaces.Identity;
using SICAF.Business.Interfaces.Instructor;
using SICAF.Business.Interfaces.Logging;
using SICAF.Business.Interfaces.Reports;
using SICAF.Business.Interfaces.Student;
using SICAF.Business.Services.Academic;
using SICAF.Business.Services.Auditing;
using SICAF.Business.Services.Catalogs;
using SICAF.Business.Services.Identity;
using SICAF.Business.Services.Instructor;
using SICAF.Business.Services.Logging;
using SICAF.Business.Services.Reports;
using SICAF.Business.Services.Student;
using SICAF.Business.Services.System;

namespace SICAF.Business;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
    {
        // add services for modules
        services.AddScoped<ISystemInfoService, SystemInfoService>();
        services.AddScoped<IAuthentication, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuditReadService, AuditReadService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IPhaseService, PhaseService>();
        services.AddScoped<ITaskManagementService, TaskManagementService>();
        services.AddScoped<IEvaluationService, EvaluationService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IReportService, ReportService>();

        // Add services for auditing and error logging
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IErrorLoggingService, ErrorLoggingService>();

        // Add services for catalog management
        services.AddScoped<ICatalogService, CatalogService>();

        return services;
    }
}