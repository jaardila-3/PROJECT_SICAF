using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class UserCourseConfiguration : BaseEntityConfiguration<UserCourse>
{
    public override void Configure(EntityTypeBuilder<UserCourse> builder)
    {
        // Llamar configuración base
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("UserCourses");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(sc => sc.UserId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario");

        builder.Property(sc => sc.CourseId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del curso");

        builder.Property(sc => sc.ParticipationType)
            .IsRequired()
            .HasComment("Tipo de participación en el curso: ESTUDIANTE o INSTRUCTOR o LIDER DE VUELO");

        builder.Property(sc => sc.WingType)
            .IsRequired(false)
            .HasComment("Tipo de ala");

        builder.Property(sc => sc.AssignmentDate)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETDATE()")
            .HasComment("Fecha de inscripción");

        builder.Property(sc => sc.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Indica si es la inscripción activa del estudiante");

        builder.Property(sc => sc.UnassignmentDate)
            .HasColumnType("datetime2")
            .HasComment("Fecha de desactivación");

        builder.Property(sc => sc.UnassignmentReason)
            .HasMaxLength(200)
            .HasComment("Razón de la desactivación");

        // CONFIGURACIÓN DE ÍNDICES

        // Índice para búsquedas por curso
        builder.HasIndex(sc => sc.CourseId)
            .HasDatabaseName("IX_UserCourses_CourseId");

        // Índice para búsquedas por estado activo
        builder.HasIndex(sc => sc.IsActive)
            .HasDatabaseName("IX_UserCourses_IsActive");

        // Índice de búsqueda por tipo de ala
        builder.HasIndex(sc => sc.WingType)
            .HasDatabaseName("IX_UserCourses_WingType");

        // Índice de búsqueda por tipo de participación
        builder.HasIndex(sc => sc.ParticipationType)
            .HasDatabaseName("IX_UserCourses_ParticipationType");

        // CONFIGURACIÓN DE RELACIONES
        builder.HasOne(sc => sc.User)
            .WithMany(u => u.UserCourses)
            .HasForeignKey(sc => sc.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_UserCourses_Users_UserId");

        builder.HasOne(sc => sc.Course)
            .WithMany(c => c.UserCourses)
            .HasForeignKey(sc => sc.CourseId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_UserCourses_CourseId");

        // Filtro global para usuarios activos
        builder.HasQueryFilter(e => e.IsActive);
    }
}