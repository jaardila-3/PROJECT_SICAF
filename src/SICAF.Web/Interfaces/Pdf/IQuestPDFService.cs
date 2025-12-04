using SICAF.Common.DTOs.Reports;

namespace SICAF.Web.Interfaces.Pdf;

/// <summary>
/// Servicio para generar PDFs de reportes académicos usando QuestPDF
/// </summary>
public interface IQuestPDFService
{
    /// <summary>
    /// Genera el PDF del reporte general académico
    /// </summary>
    /// <param name="data">Datos del reporte general</param>
    /// <returns>Byte array del PDF generado</returns>
    Task<byte[]> GenerateGeneralReportPdfAsync(GeneralReportDto data);

    /// <summary>
    /// Genera el PDF del reporte individual académico
    /// </summary>
    /// <param name="data">Datos del reporte individual</param>
    /// <returns>Byte array del PDF generado</returns>
    Task<byte[]> GenerateIndividualReportPdfAsync(IndividualReportDto data);
}