namespace SICAF.Common.Configuration.Options;

public class FeatureFlagsOptions
{
    public const string SectionName = "FeatureFlags";
    public bool DatabaseMigrateAndSeedAtStartup { get; set; } = false;
}