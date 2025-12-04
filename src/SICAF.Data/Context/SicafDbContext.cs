using Microsoft.EntityFrameworkCore;

using SICAF.Data.Entities.Academic;
using SICAF.Data.Entities.Auditing;
using SICAF.Data.Entities.Catalogs;
using SICAF.Data.Entities.Identity;
using SICAF.Data.Entities.Logging;

namespace SICAF.Data.Context;

public class SicafDbContext(DbContextOptions<SicafDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SicafDbContext).Assembly);
        /*
        ** Añade una por una de las configuraciones
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new ErrorLogConfiguration());
        modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
        ...
        */

        base.OnModelCreating(modelBuilder);
    }

    // Identity
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<ErrorLog> ErrorLogs { get; set; }

    // Catalogo
    public DbSet<MasterCatalog> MasterCatalogs { get; set; }

    // Academic
    public DbSet<AviationProfile> AviationProfiles { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<UserCourse> UserCourses { get; set; }
    public DbSet<Phase> Phases { get; set; }
    public DbSet<StudentPhaseProgress> StudentPhaseProgress { get; set; }
    public DbSet<Mission> Missions { get; set; }
    public DbSet<Tasks> Tasks { get; set; }
    public DbSet<MissionTask> MissionTasks { get; set; }
    public DbSet<StudentTaskGrade> StudentTaskGrades { get; set; }
    public DbSet<StudentMissionProgress> StudentMissionProgress { get; set; }
    public DbSet<FlightHourLog> FlightHourLogs { get; set; }
    public DbSet<StudentGradeNRedReason> StudentGradeNRedReasons { get; set; }
    public DbSet<Aircraft> Aircrafts { get; set; }
    public DbSet<StudentCommitteeRecord> StudentCommitteeRecords { get; set; }
    public DbSet<NonEvaluableMissionRecord> NonEvaluableMissionRecords { get; set; }
    public DbSet<NonEvaluableTaskGrade> NonEvaluableTaskGrades { get; set; }
    public DbSet<NonEvaluableGradeReason> NonEvaluableGradeReasons { get; set; }
}