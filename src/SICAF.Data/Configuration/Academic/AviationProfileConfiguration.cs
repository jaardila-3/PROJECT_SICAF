using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class AviationProfileConfiguration : BaseEntityConfiguration<AviationProfile>
{
    public override void Configure(EntityTypeBuilder<AviationProfile> builder)
    {
        // Llamar configuración base
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("AviationProfiles");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(e => e.UserId)
            .HasColumnType("uniqueidentifier")
            .IsRequired()
            .HasComment("ID del usuario");

        builder.Property(e => e.PID)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("PID del piloto");

        builder.Property(e => e.FlightPosition)
            .IsRequired()
            .HasMaxLength(60)
            .HasComment("Posición de vuelo");

        builder.Property(e => e.WingType)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("Tipo de ala de la aeronave");

        builder.Property(e => e.TotalFlightHours)
            .IsRequired()
            .HasComment("Horas totales de vuelo");

        // indices
        builder.HasIndex(e => e.PID)
            .IsUnique()
            .HasDatabaseName("IX_AviationProfiles_PID_Unique");

        builder.HasIndex(e => e.UserId)
            .HasDatabaseName("IX_AviationProfiles_UserId");

        builder.HasIndex(e => e.WingType)
            .HasDatabaseName("IX_AviationProfiles_WingType");

        // Relación uno a uno con User
        builder.HasOne(e => e.User)
            .WithOne(u => u.AviationProfile)
            .HasForeignKey<AviationProfile>(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_AviationProfiles_UserId");
    }
}