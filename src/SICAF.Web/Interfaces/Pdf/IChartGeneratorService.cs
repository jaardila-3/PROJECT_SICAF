using SICAF.Common.DTOs.Reports;

namespace SICAF.Web.Interfaces.Pdf;

/// <summary>
/// Servicio para generar gráficos de reportes usando ScottPlot
/// </summary>
public interface IChartGeneratorService
{
    /// <summary>
    /// Genera gráfico de distribución de calificaciones (barras verticales)
    /// </summary>
    byte[] GenerateGradeDistributionChart(GradeDistributionDto data);

    /// <summary>
    /// Genera gráfico de horas de vuelo por máquina (barras horizontales)
    /// </summary>
    byte[] GenerateMachineFlightHoursChart(List<MachineFlightHoursDto> data);

    /// <summary>
    /// Genera gráfico de horas de vuelo por instructor (barras horizontales)
    /// </summary>
    byte[] GenerateInstructorFlightHoursChart(List<InstructorFlightHoursDto> data);

    /// <summary>
    /// Genera gráfico de misiones insatisfactorias por máquina (barras horizontales)
    /// </summary>
    byte[] GenerateMachineUnsatisfactoryChart(List<MachineUnsatisfactoryMissionsDto> data);

    /// <summary>
    /// Genera gráfico de misiones insatisfactorias por instructor (barras horizontales)
    /// </summary>
    byte[] GenerateInstructorUnsatisfactoryChart(List<InstructorUnsatisfactoryMissionsDto> data);

    /// <summary>
    /// Genera gráfico de categorías N ROJA (barras verticales)
    /// </summary>
    byte[] GenerateNRedCategoriesChart(NRedReasonsDto data);
}