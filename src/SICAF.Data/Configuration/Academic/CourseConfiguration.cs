using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class CourseConfiguration : BaseEntityConfiguration<Course>
{
    public override void Configure(EntityTypeBuilder<Course> builder)
    {
        // Llamar configuración base
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("Courses", t => t.HasCheckConstraint("CK_Courses_CourseNumber", "CourseNumber >= 1 AND CourseNumber <= 999"));

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(c => c.CourseName)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Nombre del curso");

        builder.Property(c => c.CourseNumber)
            .IsRequired()
            .HasMaxLength(3)
            .HasComment("Número del curso (máximo 3 dígitos)");

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("Descripción del curso");

        builder.Property(c => c.StartDate)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasComment("Fecha de inicio del curso");

        builder.Property(c => c.EndDate)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasComment("Fecha de finalización del curso");

        // CONFIGURACIÓN DE ÍNDICES        

        // Índice único para número de curso
        builder.HasIndex(c => c.CourseNumber)
            .IsUnique()
            .HasDatabaseName("IX_Courses_CourseNumber_Unique")
            .HasFilter("IsDeleted = 0");

        // Índice para búsquedas por nombre
        builder.HasIndex(c => c.CourseName)
            .HasDatabaseName("IX_Courses_CourseName");

        // Índice para búsquedas por fechas
        builder.HasIndex(c => new { c.StartDate, c.EndDate })
            .HasDatabaseName("IX_Courses_Dates");

        // CONFIGURACIÓN DE RELACIONES
        builder.HasMany(c => c.UserCourses)
            .WithOne(sc => sc.Course)
            .HasForeignKey(sc => sc.CourseId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Courses_CourseId");
    }
}