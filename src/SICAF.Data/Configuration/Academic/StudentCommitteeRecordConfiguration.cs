using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class StudentCommitteeRecordConfiguration : BaseEntityConfiguration<StudentCommitteeRecord>
{
    public override void Configure(EntityTypeBuilder<StudentCommitteeRecord> builder)
    {
        // Llamar configuración base
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("StudentCommitteeRecords");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(mt => mt.StudentId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario con rol de estudiante");

        builder.Property(mt => mt.LeaderId)
            .IsRequired(false)
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario con rol de lider de vuelo");

        builder.Property(mt => mt.CourseId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del curso");

        builder.Property(mt => mt.PhaseId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la fase");

        builder.Property(mt => mt.CommitteeNumber)
            .IsRequired()
            .HasColumnType("int")
            .HasComment("Número de veces en comite por fase");

        builder.Property(mt => mt.Date)
            .IsRequired()
            .HasColumnType("datetime")
            .HasComment("Fecha en que fue a comité (automático)");

        builder.Property(mt => mt.ActaNumber)
            .IsRequired(false)
            .HasMaxLength(100)
            .HasComment("Número de acta");

        builder.Property(mt => mt.Reason)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Motivo de ir al comite");

        builder.Property(mt => mt.Decision)
            .IsRequired(false)
            .HasMaxLength(100)
            .HasComment("Decisión: continuar o suspender el curso");

        builder.Property(mt => mt.DecisionDate)
            .IsRequired(false)
            .HasColumnType("datetime")
            .HasComment("Fecha de la decisión");

        builder.Property(mt => mt.IsResolved)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Indica si la misión fue resuelta");

        builder.Property(mt => mt.DecisionObservations)
            .HasMaxLength(3000)
            .HasComment("Observaciones sobre la decisión");

        // Relaciones
        builder.HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentCommitteeRecords_StudentId");

        builder.HasOne(x => x.RegisteredByLeader)
            .WithMany()
            .HasForeignKey(x => x.LeaderId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentCommitteeRecords_LeaderId");

        builder.HasOne(x => x.Course)
            .WithMany()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentCommitteeRecords_CourseId");

        builder.HasOne(x => x.Phase)
            .WithMany()
            .HasForeignKey(x => x.PhaseId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentCommitteeRecords_PhaseId");

        // Índices
        builder.HasIndex(x => x.StudentId).HasDatabaseName("IX_StudentCommitteeRecords_StudentId");
        builder.HasIndex(x => x.PhaseId).HasDatabaseName("IX_StudentCommitteeRecords_PhaseId");
        builder.HasIndex(x => x.IsResolved).HasDatabaseName("IX_StudentCommitteeRecords_IsResolved");

    }
}