using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class StudentTaskGradeConfiguration : BaseEntityConfiguration<StudentTaskGrade>
{
    public override void Configure(EntityTypeBuilder<StudentTaskGrade> builder)
    {
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("StudentTaskGrades");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(stg => stg.StudentId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario con rol de estudiante");

        builder.Property(stg => stg.InstructorId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario con rol de Instructor");

        builder.Property(stg => stg.MissionId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la misión");

        builder.Property(stg => stg.TaskId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la tarea");

        builder.Property(stg => stg.StudentMissionProgressId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del progreso de la misión del estudiante");

        builder.Property(stg => stg.Grade)
            .IsRequired()
            .HasMaxLength(10)
            .HasComment("Calificación: A, B, C, N, NR (N ROJA)");

        builder.Property(stg => stg.Date)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasComment("Fecha de la calificación");

        builder.Property(stg => stg.EditCount)
            .IsRequired()
            .HasDefaultValue(0)
            .HasComment("Contador de ediciones");

        builder.Property(stg => stg.LastEditDate)
            .IsRequired(false)
            .HasColumnType("datetime2")
            .HasComment("Fecha de la ultima edición");

        // CONFIGURACIÓN DE ÍNDICES

        builder.HasIndex(stg => stg.StudentId)
            .HasDatabaseName("IX_StudentTaskGrades_StudentId");

        builder.HasIndex(stg => stg.InstructorId)
            .HasDatabaseName("IX_StudentTaskGrades_InstructorId");

        // CONFIGURACIÓN DE RELACIONES
        builder.HasOne(stg => stg.Student)
            .WithMany(s => s.StudentTaskGradesAsStudent)
            .HasForeignKey(stg => stg.StudentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentTaskGrades_StudentId");

        builder.HasOne(stg => stg.Instructor)
            .WithMany(s => s.StudentTaskGradesAsInstructor)
            .HasForeignKey(stg => stg.InstructorId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentTaskGrades_InstructorId");

        builder.HasOne(stg => stg.StudentMissionProgress)
            .WithMany(smp => smp.StudentTaskGrades)
            .HasForeignKey(stg => stg.StudentMissionProgressId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentTaskGrades_StudentMissionProgresId");

        builder.HasOne(stg => stg.Mission)
            .WithMany(m => m.StudentTaskGrades)
            .HasForeignKey(stg => stg.MissionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentTaskGrades_MissionId");

        builder.HasOne(stg => stg.Task)
            .WithMany(t => t.StudentTaskGrades)
            .HasForeignKey(stg => stg.TaskId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_StudentTaskGrades_TaskId");
    }
}