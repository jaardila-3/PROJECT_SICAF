using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Identity;

namespace SICAF.Data.Configuration.Identity;

public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
{
    public override void Configure(EntityTypeBuilder<UserRole> builder)
    {
        // Llamar configuración base
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("UserRoles");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(ur => ur.UserId)
            .HasColumnType("uniqueidentifier")
            .IsRequired()
            .HasComment("ID del usuario");

        builder.Property(ur => ur.RoleId)
            .HasColumnType("uniqueidentifier")
            .IsRequired()
            .HasComment("ID del rol");

        builder.Property(ur => ur.ExpiresAt)
            .HasColumnType("datetime")
            .HasComment("Fecha de expiración del rol (para roles temporales)");

        // CONFIGURACIÓN DE ÍNDICES
        // Índice único compuesto para evitar roles duplicados por usuario
        builder.HasIndex(ur => new { ur.UserId, ur.RoleId })
            .IsUnique()
            .HasDatabaseName("IX_UserRoles_User_Role_Unique")
            .HasFilter("IsDeleted = 0"); // Solo para registros no eliminados

        // Índice para búsquedas por usuario
        builder.HasIndex(ur => ur.UserId)
            .HasDatabaseName("IX_UserRoles_UserId");

        // Índice para búsquedas por rol
        builder.HasIndex(ur => ur.RoleId)
            .HasDatabaseName("IX_UserRoles_RoleId");

        // Índice para roles que expiran
        builder.HasIndex(ur => ur.ExpiresAt)
            .HasDatabaseName("IX_UserRoles_ExpiresAt");
    }
}