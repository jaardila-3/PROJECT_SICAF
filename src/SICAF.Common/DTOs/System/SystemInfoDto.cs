namespace SICAF.Common.DTOs.System;

public class SystemInfoDto
{
    public string ApplicationName { get; set; } = "SICAF";
    public FrameworkInfo Framework { get; set; } = new();
    public string Environment { get; set; } = string.Empty;
    public DateTime BuildDate { get; set; }
    public DateTime ServerTime { get; set; }
    public string ServerTimeZone { get; set; } = string.Empty;

    public DatabaseInfo Database { get; set; } = new();
    public DeploymentInfo Deployment { get; set; } = new();

    public class DatabaseInfo
    {
        public string Provider { get; set; } = "SQL Server";
        public string ConnectionStatus { get; set; } = string.Empty;
        public int PendingMigrations { get; set; }
    }

    public class DeploymentInfo
    {
        public string DeployedBy { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public string ReleaseNotes { get; set; } = string.Empty;
    }

    public class FrameworkInfo
    {
        public string RuntimeVersion { get; set; } = string.Empty; // ".NET 8.0.1"
        public string DotNetVersion { get; set; } = string.Empty; // "8.0.1"
        public string TargetFramework { get; set; } = string.Empty; // ".NETCoreApp,Version=v8.0"
        public string OperatingSystem { get; set; } = string.Empty;
        public string ProcessArchitecture { get; set; } = string.Empty;
        public string OSArchitecture { get; set; } = string.Empty;
        public string MachineName { get; set; } = string.Empty;
        public int ProcessId { get; set; }
        public string ProcessName { get; set; } = string.Empty;
        public bool Is64BitProcess { get; set; }
        public Dictionary<string, string> AssemblyVersions { get; set; } = new();
    }
}