using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.Configuration.Options;

public class LockoutSettings
{
    public const string SectionName = "LockoutSettings";

    public int MaxFailedAttempts { get; set; } = 3;
    public int LockoutDurationMinutes { get; set; } = 30;
    public bool EnableLockout { get; set; } = true;
}