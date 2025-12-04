using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class StudentMissionProgressConfiguration : BaseEntityConfiguration<StudentMissionProgress>
{
    public override void Configure(EntityTypeBuilder<StudentMissionProgress> builder)
    {
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("StudentMissionProgress");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(smp => smp.StudentId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario con rol de estudiante");

        builder.Property(smp => smp.InstructorId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario con rol de instructor");

        builder.Property(smp => smp.MissionId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la misión");

        builder.Property(smp => smp.PhaseId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la fase");

        builder.Property(smp => smp.CourseId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del curso");

        builder.Property(smp => smp.AircraftId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la aeronave utilizada en la misión");

        builder.Property(smp => smp.Date)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasComment("Fecha de realización de la misión");

        builder.Property(smp => smp.MissionPassed)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Indica si la misión fue aprobada");

        builder.Property(smp => smp.CriticalFailures)
            .IsRequired()
            .HasDefaultValue(0)
            .HasComment("Cantidad de fallas críticas (N-Rojas)");

        builder.Property(smp => smp.Observations)
            .HasMaxLength(2000)
            .HasComment("Observaciones adicionales");

        // CONFIGURACIÓN DE ÍNDICES
        builder.HasIndex(smp => smp.PhaseId)
            .HasDatabaseName("IX_StudentMissionProgress_PhaseId");

        builder.HasIndex(smp => smp.MissionId)
            .HasDatabaseName("IX_StudentMissionProgress_MissionId");

        builder.HasIndex(smp => smp.InstructorId)
            .HasDatabaseName("IX_StudentMissionProgress_InstructorId");

        builder.HasIndex(smp => smp.AircraftId)
            .HasDatabaseName("IX_StudentMissionProgress_AircraftId");

        // CONFIGURACIÓN DE RELACIONES
        builder.HasOne(smp => smp.Student)
            .WithMany(s => s.StudentMissionProgressesAsStudent)
            .HasForeignKey(smp => smp.StudentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentMissionProgress_StudentId");

        builder.HasOne(smp => smp.Instructor)
            .WithMany()
            .HasForeignKey(smp => smp.InstructorId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentMissionProgress_InstructorId");

        builder.HasOne(smp => smp.Mission)
            .WithMany(m => m.StudentMissionProgresses)
            .HasForeignKey(smp => smp.MissionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentMissionProgress_MissionId");

        builder.HasOne(smp => smp.Phase)
            .WithMany()
            .HasForeignKey(smp => smp.PhaseId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentMissionProgress_PhaseId");

        builder.HasOne(smp => smp.Course)
            .WithMany()
            .HasForeignKey(smp => smp.CourseId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentMissionProgress_CourseId");

        builder.HasOne(smp => smp.Aircraft)
            .WithMany()
            .HasForeignKey(smp => smp.AircraftId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentMissionProgress_AircraftId");
    }
}