using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class MissionTaskConfiguration : BaseEntityConfiguration<MissionTask>
{
    public override void Configure(EntityTypeBuilder<MissionTask> builder)
    {
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("MissionTasks");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(mt => mt.MissionId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la misión");

        builder.Property(mt => mt.TaskId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la tarea");

        builder.Property(mt => mt.IsP3Task)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Indica si esta tarea es P3 (crítica) en esta misión");

        builder.Property(mt => mt.DisplayOrder)
            .HasDefaultValue(0)
            .HasComment("Orden de visualización");

        // CONFIGURACIÓN DE ÍNDICES

        builder.HasIndex(mt => mt.MissionId).HasDatabaseName("IX_MissionTasks_MissionId");
        builder.HasIndex(mt => mt.TaskId).HasDatabaseName("IX_MissionTasks_TaskId");
        builder.HasIndex(mt => mt.IsP3Task).HasDatabaseName("IX_MissionTasks_IsP3Task");

        // CONFIGURACIÓN DE RELACIONES
        builder.HasOne(mt => mt.Mission)
            .WithMany(m => m.MissionTasks)
            .HasForeignKey(mt => mt.MissionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_MissionTasks_MissionId");

        builder.HasOne(mt => mt.Task)
            .WithMany()
            .HasForeignKey(mt => mt.TaskId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_MissionTasks_TaskId");
    }
}