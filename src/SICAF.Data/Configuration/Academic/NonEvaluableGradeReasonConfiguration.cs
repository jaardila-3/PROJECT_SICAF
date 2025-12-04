using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class NonEvaluableGradeReasonConfiguration : BaseEntityConfiguration<NonEvaluableGradeReason>
{
    public override void Configure(EntityTypeBuilder<NonEvaluableGradeReason> builder)
    {
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("NonEvaluableGradeReasons");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(negr => negr.NonEvaluableTaskGradeId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la calificación de tarea no evaluable");

        builder.Property(negr => negr.ReasonCategory)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Categoría: Mental, Fisica, Emocional");

        builder.Property(negr => negr.ReasonDescription)
            .IsRequired()
            .HasMaxLength(1000)
            .HasComment("Descripción o motivo de la calificación N (No Satisfactorio)");

        builder.Property(negr => negr.Date)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasComment("Fecha del registro del motivo");

        // CONFIGURACIÓN DE ÍNDICES
        builder.HasIndex(negr => negr.NonEvaluableTaskGradeId)
            .HasDatabaseName("IX_NonEvaluableGradeReasons_NonEvaluableTaskGradeId");

        builder.HasIndex(negr => negr.ReasonCategory)
            .HasDatabaseName("IX_NonEvaluableGradeReasons_ReasonCategory");

        // CONFIGURACIÓN DE RELACIONES
        builder.HasOne(negr => negr.NonEvaluableTaskGrade)
            .WithMany(netg => netg.NonEvaluableGradeReasons)
            .HasForeignKey(negr => negr.NonEvaluableTaskGradeId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_NonEvaluableGradeReasons_NonEvaluableTaskGradeId");
    }
}
