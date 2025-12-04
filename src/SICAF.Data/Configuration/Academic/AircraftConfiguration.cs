using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

using static SICAF.Common.Constants.AviationConstants;

namespace SICAF.Data.Configuration.Academic;

public class AircraftConfiguration : BaseEntityConfiguration<Aircraft>
{
    public override void Configure(EntityTypeBuilder<Aircraft> builder)
    {
        // Llamar configuración base
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("Aircrafts",
            t => t.HasCheckConstraint("CK_Aircrafts_WingType", $"[WingType] IN ('{WingTypes.FIXED_WING}', '{WingTypes.ROTARY_WING}')")
        );

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(e => e.AircraftType)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("Tipo de aeronave: AVION o HELICOPTERO");

        builder.Property(e => e.Registration)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("Matrícula de la aeronave sin guiones");

        builder.Property(e => e.WingType)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("Tipo de ala: FIXED_WING o ROTARY_WING");

        builder.Property(e => e.TotalFlightHours)
            .IsRequired()
            .HasComment("Horas totales de vuelo");

        // ÍNDICES
        // Índice único compuesto: Registration + WingType
        builder.HasIndex(e => new { e.Registration, e.WingType })
            .IsUnique()
            .HasDatabaseName("IX_Aircrafts_Registration_WingType_Unique")
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(e => e.AircraftType)
            .HasDatabaseName("IX_Aircrafts_AircraftType");

        builder.HasIndex(e => e.WingType)
            .HasDatabaseName("IX_Aircrafts_WingType");
    }
}