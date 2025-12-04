using QuestPDF.Fluent;

using ScottPlot;

using SICAF.Common.DTOs.Reports;
using SICAF.Web.Interfaces.Pdf;
using SICAF.Web.Models.Pdf;

namespace SICAF.Web.Services.Pdf;

public class QuestPDFService(
    ILogger<QuestPDFService> logger,
    IChartGeneratorService chartGenerator,
    IWebHostEnvironment environment
) : IQuestPDFService
{
    private readonly ILogger<QuestPDFService> _logger = logger;
    private readonly IChartGeneratorService _chartGenerator = chartGenerator;
    private readonly IWebHostEnvironment _environment = environment;

    public async Task<byte[]> GenerateGeneralReportPdfAsync(GeneralReportDto data)
    {
        try
        {
            _logger.LogInformation("Iniciando generación de PDF del reporte general para el programa {CourseName}", data.CourseName);

            // Cargar logo institucional
            var logo = await LoadLogoAsync();

            // Generar los 6 gráficos
            _logger.LogInformation("Generando gráficos del reporte general");

            var charts = new Dictionary<string, byte[]>
            {
                ["gradeDistribution"] = data.GradeDistribution != null
                    ? _chartGenerator.GenerateGradeDistributionChart(data.GradeDistribution)
                    : GenerateEmptyChartPlaceholder(),

                ["machineFlightHours"] = _chartGenerator.GenerateMachineFlightHoursChart(data.MachineFlightHours),

                ["instructorFlightHours"] = _chartGenerator.GenerateInstructorFlightHoursChart(data.InstructorFlightHours),

                ["machineUnsatisfactory"] = _chartGenerator.GenerateMachineUnsatisfactoryChart(data.MachineUnsatisfactoryMissions),

                ["instructorUnsatisfactory"] = _chartGenerator.GenerateInstructorUnsatisfactoryChart(data.InstructorUnsatisfactoryMissions),

                ["nRedCategories"] = data.NRedReasons != null
                    ? _chartGenerator.GenerateNRedCategoriesChart(data.NRedReasons)
                    : GenerateEmptyChartPlaceholder()
            };

            _logger.LogInformation("Gráficos generados exitosamente. Creando documento PDF");

            // Crear y generar documento
            var document = new GeneralReportDocument(data, logo, charts);
            var pdfBytes = document.GeneratePdf();

            _logger.LogInformation("PDF del reporte general generado exitosamente. Tamaño: {Size} bytes", pdfBytes.Length);

            return pdfBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar el PDF del reporte general para el programa {CourseName}", data.CourseName);
            throw;
        }
    }

    public async Task<byte[]> GenerateIndividualReportPdfAsync(IndividualReportDto data)
    {
        try
        {
            _logger.LogInformation("Iniciando generación de PDF del reporte individual para {StudentName}", data.FullName);

            // Cargar logo institucional
            var logo = await LoadLogoAsync();

            // Procesar foto del estudiante
            byte[]? studentPhoto = null;
            if (!string.IsNullOrEmpty(data.PhotoBase64))
            {
                studentPhoto = ConvertBase64ToBytes(data.PhotoBase64);
            }

            // Si no hay foto, generar imagen con iniciales
            studentPhoto ??= GenerateInitialsImage(data.FullName);

            _logger.LogInformation("Creando documento PDF del reporte individual");

            // Crear y generar documento
            var document = new IndividualReportDocument(data, logo, studentPhoto);
            var pdfBytes = document.GeneratePdf();

            _logger.LogInformation("PDF del reporte individual generado exitosamente. Tamaño: {Size} bytes", pdfBytes.Length);

            return pdfBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar el PDF del reporte individual para {StudentName}", data.FullName);
            throw;
        }
    }

    // Helper methods

    private async Task<byte[]> LoadLogoAsync()
    {
        try
        {
            var logoPath = Path.Combine(_environment.WebRootPath, "assets", "images", "institutional", "escudo_esavi.png");

            if (!File.Exists(logoPath))
            {
                _logger.LogWarning("Logo institucional no encontrado en {Path}. Generando placeholder", logoPath);
                return GenerateLogoPlaceholder();
            }

            return await File.ReadAllBytesAsync(logoPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar el logo institucional");
            return GenerateLogoPlaceholder();
        }
    }

    private byte[]? ConvertBase64ToBytes(string base64)
    {
        try
        {
            // Remover prefijo si existe (data:image/png;base64, o data:image/jpeg;base64,)
            var base64Data = base64;
            if (base64.Contains(","))
            {
                base64Data = base64.Split(',')[1];
            }

            return Convert.FromBase64String(base64Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al convertir base64 a bytes");
            return null;
        }
    }

    private byte[] GenerateInitialsImage(string fullName)
    {
        try
        {
            var plot = new Plot();

            // Obtener iniciales
            var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var initials = parts.Length >= 2
                ? $"{parts[0][0]}{parts[^1][0]}"
                : parts.Length == 1 ? $"{parts[0][0]}" : "??";

            // Crear imagen con fondo verde y texto blanco
            var text = plot.Add.Text(initials.ToUpper(), 0.5, 0.5);
            text.LabelFontSize = 48;
            text.LabelBold = true;
            text.LabelFontColor = Colors.White;

            plot.Axes.SetLimits(0, 1, 0, 1);
            plot.HideGrid();
            plot.Axes.Frameless();
            plot.DataBackground.Color = Color.FromHex("#409448"); // Verde institucional

            plot.ScaleFactor = 2;

            return plot.GetImage(200, 200).GetImageBytes();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar imagen de iniciales");
            return GenerateLogoPlaceholder();
        }
    }

    private static byte[] GenerateLogoPlaceholder()
    {
        var plot = new Plot();

        plot.Add.Text("ESAVI", 0.5, 0.5);
        plot.Axes.SetLimits(0, 1, 0, 1);
        plot.HideGrid();
        plot.Axes.Frameless();
        plot.DataBackground.Color = Colors.White;

        plot.ScaleFactor = 2;

        return plot.GetImage(200, 100).GetImageBytes();
    }

    private static byte[] GenerateEmptyChartPlaceholder()
    {
        var plot = new Plot();

        plot.Add.Text("Sin datos", 0.5, 0.5);
        plot.HideGrid();
        plot.Axes.Frameless();
        plot.FigureBackground.Color = Colors.White;
        plot.DataBackground.Color = Colors.White;

        plot.ScaleFactor = 2;

        return plot.GetImage(800, 400).GetImageBytes();
    }
}