using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Identity;

namespace SICAF.Data.Configuration.Identity;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        // Llamar configuración base
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("Users");

        // CONFIGURACIÓN DE PROPIEDADES REQUERIDAS
        builder.Property(u => u.DocumentType)
            .IsRequired()
            .HasMaxLength(10)
            .HasComment("Tipo de documento de identificación (CC, CE, etc.)");

        builder.Property(u => u.IdentificationNumber)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("Número del documento de identificación");

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Nombres del usuario");

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Apellidos del usuario");

        builder.Property(u => u.Nationality)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Colombia")
            .HasComment("Nacionalidad del usuario");

        builder.Property(u => u.BloodType)
            .IsRequired()
            .HasMaxLength(5)
            .HasComment("Factor RH de sangre (O+, O-, A+, A-, B+, B-, AB+, AB-)");

        builder.Property(u => u.BirthDate)
            .IsRequired()
            .HasColumnType("date")
            .HasComment("Fecha de nacimiento del usuario");

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Nombre de usuario único para login");

        builder.Property(u => u.Email)
            .IsRequired(false)
            .HasMaxLength(100)
            .HasComment("Correo electrónico del usuario");

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20)
            .HasComment("Número de teléfono del usuario");

        builder.Property(u => u.Grade)
            .HasMaxLength(50)
            .HasComment("Grado policial del usuario");

        builder.Property(u => u.SeniorityOrder)
            .IsRequired(false)
            .HasComment("Orden de antiguedad del usuario");

        builder.Property(u => u.Force)
            .HasMaxLength(100)
            .HasComment("Fuerza a la que pertenece (Policía Nacional, Ejercito, etc.)");

        builder.Property(u => u.PhotoData)
            .HasColumnType("varbinary(max)")
            .HasComment("Fotografía del usuario en formato binario");

        builder.Property(u => u.PhotoContentType)
            .HasMaxLength(50)
            .HasComment("Tipo de contenido MIME de la fotografía");

        builder.Property(u => u.PhotoFileName)
            .HasMaxLength(200)
            .HasComment("Nombre de archivo con la extensión");

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("Hash de la contraseña del usuario");

        builder.Property(u => u.LockoutEnd)
            .HasColumnType("datetime")
            .HasComment("Fecha y hora hasta cuando el usuario está bloqueado");

        builder.Property(u => u.LockoutReason)
            .HasMaxLength(500)
            .HasComment("Razón del bloqueo del usuario");

        builder.Property(u => u.AccessFailedCount)
            .IsRequired()
            .HasDefaultValue(0)
            .HasComment("Número de intentos fallidos de autenticación");

        builder.Property(u => u.IsPasswordSetByAdmin)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Indica si la contraseña del usuario fue establecida por el administrador");

        builder.Property(u => u.PasswordChangeDate)
            .HasColumnType("datetime")
            .HasComment("Fecha y hora de cambio de contraseña del usuario");

        // CONFIGURACIÓN DE ÍNDICES ÚNICOS
        // Índice único compuesto para documento
        builder.HasIndex(u => new { u.DocumentType, u.IdentificationNumber })
            .IsUnique()
            .HasDatabaseName("IX_Users_Document_Unique")
            .HasFilter("IsDeleted = 0"); // Solo para registros no eliminados

        // Índice único para nombre de usuario
        builder.HasIndex(u => u.Username)
            .IsUnique()
            .HasDatabaseName("IX_Users_Username_Unique")
            .HasFilter("IsDeleted = 0");

        // Índice único para email
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email_Unique")
            .HasFilter("[Email] IS NOT NULL AND IsDeleted = 0");

        // Índice único para el grado y la antiguedad en el grado
        builder.HasIndex(u => new { u.Grade, u.SeniorityOrder })
            .IsUnique()
            .HasDatabaseName("IX_Users_Grade_SeniorityOrder_Unique")
            .HasFilter("[Grade] IS NOT NULL AND [SeniorityOrder] IS NOT NULL AND IsDeleted = 0");

        // ÍNDICES DE RENDIMIENTO
        // Índice para búsquedas por nombre completo
        builder.HasIndex(u => new { u.Name, u.LastName })
            .HasDatabaseName("IX_Users_FullName");

        // Índice para fecha de nacimiento
        builder.HasIndex(u => u.BirthDate)
            .HasDatabaseName("IX_Users_BirthDate");

        // Índice para usuarios bloqueados
        builder.HasIndex(u => u.LockoutEnd)
            .HasDatabaseName("IX_Users_LockoutEnd");

        // CONFIGURACIÓN DE RELACIONES
        // Relación uno a muchos con UserRoles
        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_UserRoles_UserId");

        // Relación uno a muchos con UserCourses
        builder.HasMany(u => u.UserCourses)
            .WithOne(sc => sc.User)
            .HasForeignKey(sc => sc.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Users_UserId");
    }
}