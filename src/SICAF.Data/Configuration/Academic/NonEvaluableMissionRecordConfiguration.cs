using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class NonEvaluableMissionRecordConfiguration : BaseEntityConfiguration<NonEvaluableMissionRecord>
{
    public override void Configure(EntityTypeBuilder<NonEvaluableMissionRecord> builder)
    {
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("NonEvaluableMissionRecords");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(nemr => nemr.StudentId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario con rol de estudiante");

        builder.Property(nemr => nemr.InstructorId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario con rol de instructor");

        builder.Property(nemr => nemr.PhaseId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la fase");

        builder.Property(nemr => nemr.CourseId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del curso");

        builder.Property(nemr => nemr.AircraftId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la aeronave utilizada en la misión");

        builder.Property(nemr => nemr.Date)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasComment("Fecha de realización de la misión no evaluable");

        builder.Property(nemr => nemr.NonEvaluableMissionNumber)
            .IsRequired()
            .HasComment("Número de misión no evaluable (1, 2, 3, etc. para MNE1, MNE2, MNE3...)");

        builder.Property(nemr => nemr.Observations)
            .IsRequired()
            .HasMaxLength(2000)
            .HasComment("Observaciones generales de la misión no evaluable");

        builder.Property(nemr => nemr.MachineFlightHours)
            .IsRequired()
            .HasComment("Horas de vuelo máquina");

        builder.Property(nemr => nemr.ManFlightHours)
            .IsRequired()
            .HasComment("Horas de vuelo máquina");

        // CONFIGURACIÓN DE ÍNDICES
        builder.HasIndex(nemr => nemr.StudentId)
            .HasDatabaseName("IX_NonEvaluableMissionRecords_StudentId");

        builder.HasIndex(nemr => nemr.PhaseId)
            .HasDatabaseName("IX_NonEvaluableMissionRecords_PhaseId");

        builder.HasIndex(nemr => nemr.InstructorId)
            .HasDatabaseName("IX_NonEvaluableMissionRecords_InstructorId");

        builder.HasIndex(nemr => nemr.AircraftId)
            .HasDatabaseName("IX_NonEvaluableMissionRecords_AircraftId");

        builder.HasIndex(nemr => nemr.CourseId)
            .HasDatabaseName("IX_NonEvaluableMissionRecords_CourseId");

        // ÍNDICE ÚNICO: Un estudiante no puede tener múltiples registros para el mismo número de misión no evaluable en la fase
        builder.HasIndex(nemr => new { nemr.StudentId, nemr.CourseId, nemr.NonEvaluableMissionNumber, nemr.PhaseId })
            .IsUnique()
            .HasDatabaseName("IX_NonEvaluableMissionRecords_Student_Course_Phase_Number_Unique")
            .HasFilter("[IsDeleted] = 0");

        // CONFIGURACIÓN DE RELACIONES
        builder.HasOne(nemr => nemr.Student)
            .WithMany()
            .HasForeignKey(nemr => nemr.StudentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_NonEvaluableMissionRecords_StudentId");

        builder.HasOne(nemr => nemr.Instructor)
            .WithMany()
            .HasForeignKey(nemr => nemr.InstructorId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_NonEvaluableMissionRecords_InstructorId");

        builder.HasOne(nemr => nemr.Phase)
            .WithMany()
            .HasForeignKey(nemr => nemr.PhaseId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_NonEvaluableMissionRecords_PhaseId");

        builder.HasOne(nemr => nemr.Course)
            .WithMany()
            .HasForeignKey(nemr => nemr.CourseId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_NonEvaluableMissionRecords_CourseId");

        builder.HasOne(nemr => nemr.Aircraft)
            .WithMany()
            .HasForeignKey(nemr => nemr.AircraftId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_NonEvaluableMissionRecords_AircraftId");
    }
}
