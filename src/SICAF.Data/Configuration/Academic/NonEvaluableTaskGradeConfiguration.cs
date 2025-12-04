using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class NonEvaluableTaskGradeConfiguration : BaseEntityConfiguration<NonEvaluableTaskGrade>
{
    public override void Configure(EntityTypeBuilder<NonEvaluableTaskGrade> builder)
    {
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("NonEvaluableTaskGrades");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(netg => netg.StudentId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario con rol de estudiante");

        builder.Property(netg => netg.InstructorId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario con rol de instructor");

        builder.Property(netg => netg.TaskId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la tarea");

        builder.Property(netg => netg.NonEvaluableMissionRecordId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del registro de misión no evaluable");

        builder.Property(netg => netg.Grade)
            .IsRequired()
            .HasMaxLength(10)
            .HasComment("Calificación: A, B, C, N, DM, SC (NO permite NR - N-Roja)");

        builder.Property(netg => netg.Date)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasComment("Fecha de la calificación");

        builder.Property(nemr => nemr.EditCount)
        .IsRequired()
        .HasDefaultValue(0)
        .HasComment("Contador de ediciones (máximo 2)");

        builder.Property(nemr => nemr.LastEditDate)
            .IsRequired(false)
            .HasColumnType("datetime2")
            .HasComment("Fecha de la última edición");

        // CONFIGURACIÓN DE ÍNDICES
        builder.HasIndex(netg => netg.StudentId)
            .HasDatabaseName("IX_NonEvaluableTaskGrades_StudentId");

        builder.HasIndex(netg => netg.InstructorId)
            .HasDatabaseName("IX_NonEvaluableTaskGrades_InstructorId");

        builder.HasIndex(netg => netg.NonEvaluableMissionRecordId)
            .HasDatabaseName("IX_NonEvaluableTaskGrades_NonEvaluableMissionRecordId");

        builder.HasIndex(netg => netg.TaskId)
            .HasDatabaseName("IX_NonEvaluableTaskGrades_TaskId");

        // CONFIGURACIÓN DE RELACIONES
        builder.HasOne(netg => netg.Student)
            .WithMany()
            .HasForeignKey(netg => netg.StudentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_NonEvaluableTaskGrades_StudentId");

        builder.HasOne(netg => netg.Instructor)
            .WithMany()
            .HasForeignKey(netg => netg.InstructorId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_NonEvaluableTaskGrades_InstructorId");

        builder.HasOne(netg => netg.NonEvaluableMissionRecord)
            .WithMany(nemr => nemr.NonEvaluableTaskGrades)
            .HasForeignKey(netg => netg.NonEvaluableMissionRecordId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_NonEvaluableTaskGrades_NonEvaluableMissionRecordId");

        builder.HasOne(netg => netg.Task)
            .WithMany()
            .HasForeignKey(netg => netg.TaskId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_NonEvaluableTaskGrades_TaskId");
    }
}
