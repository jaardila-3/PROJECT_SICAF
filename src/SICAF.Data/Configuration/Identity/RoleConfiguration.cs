using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Identity;

namespace SICAF.Data.Configuration.Identity;

public class RoleConfiguration : BaseEntityConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        // Llamar configuración base
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("Roles");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Nombre del rol");

        builder.Property(r => r.Description)
            .HasMaxLength(255)
            .HasComment("Descripción del rol");

        builder.Property(r => r.IsSystemRole)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Indica si el rol es un rol del sistema");

        // CONFIGURACIÓN DE ÍNDICES
        // Índice único para nombre del rol
        builder.HasIndex(r => r.Name)
            .IsUnique()
            .HasDatabaseName("IX_Roles_Name_Unique")
            .HasFilter("IsDeleted = 0"); // Solo para registros no eliminados

        // CONFIGURACIÓN DE RELACIONES
        // Relación uno a muchos con UserRoles
        builder.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_UserRoles_Roles");
    }
}