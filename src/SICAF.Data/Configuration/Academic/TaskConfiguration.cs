using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class TaskConfiguration : BaseEntityConfiguration<Tasks>
{
    public override void Configure(EntityTypeBuilder<Tasks> builder)
    {
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("Tasks");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(wt => wt.Code)
            .IsRequired()
            .HasComment("Código único de la tarea");

        builder.Property(wt => wt.Name)
            .IsRequired()
            .HasMaxLength(300)
            .HasComment("Nombre descriptivo de la tarea");

        builder.Property(wt => wt.WingType)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("Tipo de ala: FIJA o ROTATORIA");

        // CONFIGURACIÓN DE ÍNDICES

        // Código único por tipo de ala
        builder.HasIndex(wt => new { wt.Code, wt.WingType })
            .IsUnique()
            .HasDatabaseName("IX_Tasks_Code_WingType_Unique")
            .HasFilter("IsDeleted = 0");

        builder.HasIndex(wt => wt.WingType)
            .HasDatabaseName("IX_Tasks_WingType");
    }
}