using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Academic;

namespace SICAF.Data.Configuration.Academic;

public class FlightHourLogConfiguration : BaseEntityConfiguration<FlightHourLog>
{
    public override void Configure(EntityTypeBuilder<FlightHourLog> builder)
    {
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("FlightHourLogs");

        // CONFIGURACIÓN DE PROPIEDADES
        builder.Property(fhl => fhl.UserId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario");

        builder.Property(fhl => fhl.CourseId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del curso");

        builder.Property(fhl => fhl.MissionId)
            .IsRequired(false) // Nullable para misiones no evaluables
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la misión (nullable para misiones no evaluables)");

        builder.Property(fhl => fhl.NonEvaluableMissionId)
            .IsRequired(false) // Nullable para misiones evaluables
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la misión no evaluable (nullable para misiones evaluables)");

        builder.Property(fhl => fhl.AircraftId)
            .IsRequired()
            .HasColumnType("uniqueidentifier")
            .HasComment("ID de la aeronave utilizada");

        builder.Property(fhl => fhl.MachineFlightHours)
            .IsRequired()
            .HasPrecision(10, 2)
            .HasComment("Horas de vuelo registrada en la aeronave");

        builder.Property(fhl => fhl.ManFlightHours)
            .IsRequired()
            .HasPrecision(10, 2)
            .HasComment("Horas de vuelo registrada para el hombre (MachineFlightHours x 1.3)");

        builder.Property(fhl => fhl.SilaboFlightHours)
            .IsRequired()
            .HasPrecision(10, 2)
            .HasComment("Horas de vuelo en semana");

        builder.Property(fhl => fhl.Role)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Rol del usuario: Estudiante, Instructor");

        builder.Property(fhl => fhl.Observations)
            .HasMaxLength(1000)
            .HasComment("Observaciones adicionales");

        builder.Property(fhl => fhl.Date)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasComment("Fecha del registro");

        // CONFIGURACIÓN DE ÍNDICES
        builder.HasIndex(fhl => fhl.UserId)
            .HasDatabaseName("IX_FlightHourLogs_UserId");

        builder.HasIndex(fhl => fhl.AircraftId)
            .HasDatabaseName("IX_FlightHourLogs_AircraftId");

        // CONFIGURACIÓN DE RELACIONES
        builder.HasOne(fhl => fhl.User)
            .WithMany(u => u.FlightHourLogs)
            .HasForeignKey(fhl => fhl.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_FlightHourLogs_UserId");

        builder.HasOne(fhl => fhl.Course)
            .WithMany()
            .HasForeignKey(fhl => fhl.CourseId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_FlightHourLogs_CourseId");

        builder.HasOne(fhl => fhl.Mission)
            .WithMany(m => m.FlightHourLogs)
            .HasForeignKey(fhl => fhl.MissionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false) // Opcional para misiones no evaluables
            .HasConstraintName("FK_FlightHourLogs_MissionId");

        builder.HasOne(fhl => fhl.NonEvaluableMission)
            .WithMany()
            .HasForeignKey(fhl => fhl.NonEvaluableMissionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false) // Opcional para misiones evaluables
            .HasConstraintName("FK_FlightHourLogs_NonEvaluableMissionId");

        builder.HasOne(fhl => fhl.Aircraft)
            .WithMany()
            .HasForeignKey(fhl => fhl.AircraftId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_FlightHourLogs_AircraftId");
    }
}