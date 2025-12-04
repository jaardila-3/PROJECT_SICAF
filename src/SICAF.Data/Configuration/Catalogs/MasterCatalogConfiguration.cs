using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Catalogs;

namespace SICAF.Data.Configuration.Catalogs;

public class MasterCatalogConfiguration : BaseEntityConfiguration<MasterCatalog>
{
    public override void Configure(EntityTypeBuilder<MasterCatalog> builder)
    {
        // Llamar configuración base
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("MasterCatalogs");

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(10)
            .HasComment("Código del catalogo");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Nombre del catalogo");

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .HasComment("Descripción del catalogo");

        builder.Property(x => x.DisplayOrder)
            .IsRequired()
            .HasDefaultValue(0)
            .HasComment("Orden de visulización");

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasComment("Fecha de creación del registro");

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Usuario que creo el registro");

        // Indices
        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName($"IX_MasterCatalogs_Code_Unique");

        builder.HasIndex(e => new { e.CatalogType, e.Code })
            .IsUnique()
            .HasDatabaseName("IX_MasterCatalogs_Type_Code_Unique");

        builder.HasIndex(x => x.DisplayOrder)
            .HasDatabaseName($"IX_MasterCatalogs_DisplayOrder");

        builder.HasIndex(e => e.CatalogType)
            .HasDatabaseName("IX_MasterCatalogs_CatalogType");
    }
}