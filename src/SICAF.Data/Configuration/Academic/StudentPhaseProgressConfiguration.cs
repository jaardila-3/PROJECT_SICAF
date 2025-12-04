using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class StudentPhaseProgressConfiguration : BaseEntityConfiguration<StudentPhaseProgress>
{
    public override void Configure(EntityTypeBuilder<StudentPhaseProgress> builder)
    {
        // Llamar configuración base
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("StudentPhaseProgress");

        // Propiedades
        builder.Property(p => p.CourseId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del curso");

        builder.Property(p => p.PhaseId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la fase");

        builder.Property(p => p.StudentId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario con rol de estudiante");

        builder.Property(p => p.LeaderId)
            .IsRequired(false)
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario con participación de líder de vuelo");

        builder.Property(p => p.PreviousPhaseId)
            .IsRequired(false)
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la fase anterior");

        builder.Property(p => p.NextPhaseId)
            .IsRequired(false)
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la fase siguiente");

        builder.Property(p => p.IsCurrentPhase)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Indica si es la fase actual del estudiante");

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Estado de la fase (EN PROCESO, COMPLETADA, FALLIDA, SUSPENDIDA)");

        builder.Property(p => p.StartDate)
            .HasColumnType("datetime2")
            .HasComment("Fecha de inicio de la fase");

        builder.Property(p => p.EndDate)
            .HasColumnType("datetime2")
            .HasComment("Fecha de finalización de la fase");

        builder.Property(p => p.CompletedMissions)
            .IsRequired()
            .HasDefaultValue(0)
            .HasComment("Cantidad de misiones completadas");

        builder.Property(p => p.FailedMissions)
            .IsRequired()
            .HasDefaultValue(0)
            .HasComment("Cantidad de misiones fallidas");

        builder.Property(p => p.PhasePassed)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Indica si el estudiante ha pasado la fase");

        builder.Property(p => p.Observations)
            .HasMaxLength(2000)
            .HasComment("Observaciones");

        builder.Property(spe => spe.IsSuspended)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Indica si el estudiante está suspendido de la fase");

        builder.Property(spe => spe.SuspensionDate)
            .HasColumnType("datetime2")
            .HasComment("Fecha de suspensión");

        builder.Property(spe => spe.SuspensionReason)
            .HasMaxLength(500)
            .HasComment("Razón de la suspensión");

        // Índices
        builder.HasIndex(p => p.CourseId).HasDatabaseName("IX_StudentPhaseProgress_CourseId");
        builder.HasIndex(p => p.PhaseId).HasDatabaseName("IX_StudentPhaseProgress_PhaseId");
        builder.HasIndex(p => p.StudentId).HasDatabaseName("IX_StudentPhaseProgress_StudentId");
        builder.HasIndex(p => p.LeaderId).HasDatabaseName("IX_StudentPhaseProgress_LeaderId").HasFilter("[LeaderId] IS NOT NULL"); ;
        builder.HasIndex(p => p.Status).HasDatabaseName("IX_StudentPhaseProgress_Status");

        // CONFIGURACIÓN DE RELACIONES
        builder.HasOne(spp => spp.Student)
            .WithMany(s => s.StudentPhaseProgresses)
            .HasForeignKey(spp => spp.StudentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentPhaseProgress_StudentId");

        builder.HasOne(spp => spp.Leader)
            .WithMany()
            .HasForeignKey(spp => spp.LeaderId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_StudentPhaseProgress_LeaderId");

        builder.HasOne(spp => spp.Phase)
            .WithMany(p => p.StudentPhaseProgresses)
            .HasForeignKey(spp => spp.PhaseId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentPhaseProgress_PhaseId");

        builder.HasOne(spp => spp.Course)
            .WithMany(c => c.StudentPhaseProgresses)
            .HasForeignKey(spp => spp.CourseId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentPhaseProgress_CourseId");
    }
}