using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class MissionConfiguration : BaseEntityConfiguration<Mission>
{
    public override void Configure(EntityTypeBuilder<Mission> builder)
    {
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("Missions");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(fhl => fhl.PhaseId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la fase");

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Nombre de la misión");

        builder.Property(m => m.MissionNumber)
            .IsRequired()
            .HasComment("Número secuencial de la misión (1-85)");

        builder.Property(m => m.FlightHours)
            .IsRequired()
            .HasPrecision(10, 2)
            .HasComment("Horas de vuelo requeridas para la misión");

        builder.Property(m => m.WingType)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("Tipo de ala: FIJA o ROTATORIA");

        // CONFIGURACIÓN DE ÍNDICES

        // Número de misión único por fase
        builder.HasIndex(m => new { m.PhaseId, m.MissionNumber })
            .IsUnique()
            .HasDatabaseName("IX_Missions_PhaseId_MissionNumber_Unique")
            .HasFilter("IsDeleted = 0");

        builder.HasIndex(m => m.MissionNumber)
            .HasDatabaseName("IX_Missions_MissionNumber");

        builder.HasIndex(m => m.WingType)
            .HasDatabaseName("IX_Missions_WingType");

        // CONFIGURACIÓN DE RELACIONES
        builder.HasOne(m => m.Phase)
            .WithMany(p => p.Missions)
            .HasForeignKey(m => m.PhaseId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Missions_PhaseId");
    }
}