using FluentValidation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SICAF.Common.Configuration.Options;
using SICAF.Common.DTOs.Academic;
using SICAF.Common.DTOs.Identity;
using SICAF.Common.DTOs.Instructor;
using SICAF.Common.Interfaces;
using SICAF.Common.Security;
using SICAF.Common.Validators.Academic;
using SICAF.Common.Validators.Identity;
using SICAF.Common.Validators.Instructor;

namespace SICAF.Common;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterCommonServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure specific settings
        services.AddOptions<AdminSettings>()
        .Bind(configuration.GetSection(AdminSettings.SectionName));

        services.AddOptions<PasswordHasherOptions>()
        .Bind(configuration.GetSection(PasswordHasherOptions.SectionName))
        .ValidateDataAnnotations()
        .ValidateOnStart();

        services.AddOptions<FeatureFlagsOptions>()
        .Bind(configuration.GetSection(FeatureFlagsOptions.SectionName));

        services.AddOptions<LockoutSettings>()
        .Bind(configuration.GetSection(LockoutSettings.SectionName));

        services.AddOptions<ImageValidationOptions>()
        .Bind(configuration.GetSection(ImageValidationOptions.SectionName))
        .ValidateDataAnnotations()
        .ValidateOnStart();

        services.AddOptions<CommitteeOptions>()
        .Bind(configuration.GetSection(CommitteeOptions.SectionName));

        // Add FluentValidation services
        services.AddTransient<IValidator<LoginDto>, LoginValidator>();
        services.AddTransient<IValidator<RegisterDto>, RegisterValidator>();
        services.AddTransient<IValidator<UpdateDto>, UpdateValidator>();
        services.AddTransient<IValidator<ChangePasswordDto>, ChangePasswordValidator>();
        services.AddTransient<IValidator<CreateCourseDto>, CreateCourseValidator>();
        services.AddTransient<IValidator<UpdateTasksDto>, UpdateTasksDtoValidator>();
        services.AddTransient<IValidator<SaveMissionEvaluationDto>, SaveMissionEvaluationValidator>();
        services.AddTransient<IValidator<TaskGradeInputDto>, TaskGradeInputValidator>();
        services.AddTransient<IValidator<NRedReasonDto>, NRedReasonValidator>();

        // Add services
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }
}