using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class StudentGradeNRedReasonConfiguration : BaseEntityConfiguration<StudentGradeNRedReason>
{
    public override void Configure(EntityTypeBuilder<StudentGradeNRedReason> builder)
    {
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("StudentGradeNRedReasons");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(stg => stg.StudentTaskGradeId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la calificación del estudiante");

        builder.Property(stg => stg.ReasonCategory)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Categoría: Mental, Fisica, Emocional");

        builder.Property(fhl => fhl.ReasonDescription)
            .IsRequired()
            .HasMaxLength(1000)
            .HasComment("Descripción o motivo de la N-Roja");

        builder.Property(stg => stg.Date)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasComment("Fecha del motivo de la N-Roja");

        // CONFIGURACIÓN DE ÍNDICES

        builder.HasIndex(stg => stg.StudentTaskGradeId)
            .HasDatabaseName("IX_StudentGradeNRedReason_StudentTaskGradeId");

        builder.HasIndex(stg => stg.ReasonCategory)
            .HasDatabaseName("IX_StudentGradeNRedReason_ReasonCategory");

        // CONFIGURACIÓN DE RELACIONES
        builder.HasOne(stg => stg.StudentTaskGrade)
            .WithMany(s => s.StudentGradeNRedReasons)
            .HasForeignKey(stg => stg.StudentTaskGradeId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentGradeNRedReason_StudentTaskGradeId");
    }
}