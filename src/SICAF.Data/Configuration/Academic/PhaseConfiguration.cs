using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class PhaseConfiguration : BaseEntityConfiguration<Phase>
{
    public override void Configure(EntityTypeBuilder<Phase> builder)
    {
        // Llamar configuración base
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("Phases");

        // Propiedades
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(true) // Para caracteres especiales
            .HasComment("Nombre de la fase");

        builder.Property(p => p.PhaseNumber)
            .IsRequired()
            .HasColumnType("int")
            .HasComment("Número de fase (secuencial)");

        builder.Property(p => p.TotalMissions)
            .IsRequired()
            .HasColumnType("int")
            .HasComment("Total de misiones de la fase");

        builder.Property(p => p.WingType)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("Tipo de ala: FIJA o ROTATORIA");

        builder.Property(p => p.PrerequisitePhaseId)
            .HasColumnType("uniqueidentifier")
            .IsRequired(false)
            .HasComment("ID de la fase prerrequisito");

        // Índices
        // Número de fase único por tipo de ala
        builder.HasIndex(p => new { p.PhaseNumber, p.WingType })
            .IsUnique()
            .HasDatabaseName("IX_Phases_Number_WingType_Unique")
            .HasFilter("IsDeleted = 0");

        builder.HasIndex(p => p.Name)
            .HasDatabaseName("IX_Phase_Name");

        builder.HasIndex(p => p.PhaseNumber)
            .HasDatabaseName("IX_Phase_PhaseNumber");

        builder.HasIndex(p => p.WingType)
            .HasDatabaseName("IX_Phases_WingType");

        builder.HasIndex(p => p.PrerequisitePhaseId)
            .HasDatabaseName("IX_Phase_PrerequisitePhaseId");

        // Relaciones
        // Auto-relación para prerrequisitos
        builder.HasOne(p => p.PrerequisitePhase)
            .WithMany(p => p.DependentPhases)
            .HasForeignKey(p => p.PrerequisitePhaseId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Phases_PrerequisitePhaseId");

    }
}