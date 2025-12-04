using ScottPlot;

using SICAF.Common.DTOs.Reports;
using SICAF.Web.Interfaces.Pdf;

namespace SICAF.Web.Services.Pdf;

public class ChartGeneratorService : IChartGeneratorService
{
    private const int ChartWidth = 800;
    private const int ChartHeight = 400;

    // Paleta de colores institucional
    private static class InstitutionalColors
    {
        // Verdes
        public static readonly Color GreenPrimary = Color.FromHex("#409448");
        public static readonly Color GreenMedium = Color.FromHex("#4DB357");
        public static readonly Color GreenLight = Color.FromHex("#6DC075");
        public static readonly Color GreenVeryLight = Color.FromHex("#7BCB62");
        public static readonly Color GreenPastel = Color.FromHex("#B5E2A7");

        // Secundarios
        public static readonly Color Cyan = Color.FromHex("#62CBB8");
        public static readonly Color GreenBlue = Color.FromHex("#4AC27E");

        // Alertas
        public static readonly Color OrangeLight = Color.FromHex("#F87C63");
        public static readonly Color Red = Color.FromHex("#D0372F");
        public static readonly Color OrangeDark = Color.FromHex("#C2744A");
        public static readonly Color Beige = Color.FromHex("#D6A185");

        // Arrays para uso en gráficos
        public static readonly Color[] GreenGradient = [GreenPrimary, GreenMedium, GreenLight, GreenVeryLight, GreenPastel, Cyan, GreenBlue];
        public static readonly Color[] OrangeGradient = [OrangeLight, OrangeDark, Beige];
        public static readonly Color[] GradeColors = [GreenPrimary, GreenMedium, GreenLight, OrangeLight, Red];
    }

    public byte[] GenerateGradeDistributionChart(GradeDistributionDto data)
    {
        var plot = new Plot();

        double[] values = [data.GradeA, data.GradeB, data.GradeC, data.GradeN, data.GradeNR];
        string[] labels = ["A", "B", "C", "N", "N ROJA"];

        var bars = plot.Add.Bars(values);

        // Aplicar colores individuales
        for (int i = 0; i < bars.Bars.Count; i++)
        {
            bars.Bars[i].FillColor = InstitutionalColors.GradeColors[i];
        }

        // Agregar etiquetas con valores
        for (int i = 0; i < values.Length; i++)
        {
            var text = plot.Add.Text(values[i].ToString(), i, values[i]);
            text.LabelFontSize = 11;
            text.LabelBold = true;
            text.LabelFontColor = Colors.Black;
            text.OffsetY = 0;
            text.LabelAlignment = Alignment.LowerCenter;
        }

        // Configurar ejes
        plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
            [0, 1, 2, 3, 4],
            labels
        );

        plot.Axes.Left.Label.Text = "Cantidad";
        plot.Axes.Bottom.Label.Text = "Calificación";

        // Añadir margen superior para las etiquetas (20% extra)
        plot.Axes.Margins(top: 0.20);

        // Estilo
        //plot.Title("Distribución de Calificaciones");
        plot.FigureBackground.Color = Colors.White;
        plot.DataBackground.Color = Colors.White;

        // Tamaño
        plot.ScaleFactor = 2;

        return plot.GetImage(ChartWidth, ChartHeight).GetImageBytes();
    }

    public byte[] GenerateMachineFlightHoursChart(List<MachineFlightHoursDto> data)
    {
        if (data.Count == 0)
            return GenerateEmptyChart("Sin datos de horas de vuelo por Aeronave");

        var plot = new Plot();

        double[] values = data.Select(x => x.TotalHours).ToArray();
        string[] labels = data.Select(x => x.AircraftRegistration).ToArray();

        var bars = plot.Add.Bars(values);
        bars.Horizontal = true;

        // Aplicar colores del degradado verde
        for (int i = 0; i < bars.Bars.Count; i++)
        {
            bars.Bars[i].FillColor = InstitutionalColors.GreenGradient[i % InstitutionalColors.GreenGradient.Length];
        }

        // Agregar etiquetas con valores
        for (int i = 0; i < values.Length; i++)
        {
            var text = plot.Add.Text($"{values[i]:F1} hrs", values[i], i);
            text.LabelFontSize = 10;
            text.LabelBold = true;
            text.LabelFontColor = Colors.Black;
            text.OffsetX = 10;
            text.LabelAlignment = Alignment.MiddleLeft;
        }

        // Configurar ejes
        plot.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
            Enumerable.Range(0, labels.Length).Select(i => (double)i).ToArray(),
            labels
        );

        plot.Axes.Bottom.Label.Text = "Horas de Vuelo";

        // Añadir margen derecho para las etiquetas
        plot.Axes.Margins(right: 0.20);

        // Estilo
        //plot.Title("Horas de Vuelo por Aeronave");
        plot.FigureBackground.Color = Colors.White;
        plot.DataBackground.Color = Colors.White;

        // Tamaño
        plot.ScaleFactor = 2;

        return plot.GetImage(ChartWidth, ChartHeight).GetImageBytes();
    }

    public byte[] GenerateInstructorFlightHoursChart(List<InstructorFlightHoursDto> data)
    {
        if (data.Count == 0)
            return GenerateEmptyChart("Sin datos de horas de vuelo por instructor");

        var plot = new Plot();

        double[] values = data.Select(x => x.TotalHours).ToArray();
        string[] labels = data.Select(x => TruncateText(x.InstructorName, 20)).ToArray();

        var bars = plot.Add.Bars(values);
        bars.Horizontal = true;

        // Aplicar colores del degradado verde
        for (int i = 0; i < bars.Bars.Count; i++)
        {
            bars.Bars[i].FillColor = InstitutionalColors.GreenGradient[i % InstitutionalColors.GreenGradient.Length];
        }

        // Agregar etiquetas con valores
        for (int i = 0; i < values.Length; i++)
        {
            var text = plot.Add.Text($"{values[i]:F1} hrs", values[i], i);
            text.LabelFontSize = 10;
            text.LabelBold = true;
            text.LabelFontColor = Colors.Black;
            text.OffsetX = 10;
            text.LabelAlignment = Alignment.MiddleLeft;
        }

        // Configurar ejes
        plot.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
            Enumerable.Range(0, labels.Length).Select(i => (double)i).ToArray(),
            labels
        );

        plot.Axes.Bottom.Label.Text = "Horas de Vuelo";

        // Añadir margen derecho para las etiquetas
        plot.Axes.Margins(right: 0.25);

        // Estilo
        //plot.Title("Horas de Vuelo por Instructor");
        plot.FigureBackground.Color = Colors.White;
        plot.DataBackground.Color = Colors.White;

        // Tamaño
        plot.ScaleFactor = 2;

        return plot.GetImage(ChartWidth, ChartHeight).GetImageBytes();
    }

    public byte[] GenerateMachineUnsatisfactoryChart(List<MachineUnsatisfactoryMissionsDto> data)
    {
        if (data.Count == 0)
            return GenerateEmptyChart("Sin datos de misiones insatisfactorias por Aeronave");

        var plot = new Plot();

        double[] values = data.Select(x => (double)x.UnsatisfactoryCount).ToArray();
        string[] labels = data.Select(x => x.AircraftRegistration).ToArray();

        var bars = plot.Add.Bars(values);
        bars.Horizontal = true;

        // Aplicar color naranja
        foreach (var bar in bars.Bars)
        {
            bar.FillColor = InstitutionalColors.OrangeLight;
        }

        // Agregar etiquetas con valores
        for (int i = 0; i < values.Length; i++)
        {
            var text = plot.Add.Text(values[i].ToString(), values[i], i);
            text.LabelFontSize = 10;
            text.LabelBold = true;
            text.LabelFontColor = Colors.Black;
            text.OffsetX = 10;
            text.LabelAlignment = Alignment.MiddleLeft;
        }

        // Configurar ejes
        plot.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
            Enumerable.Range(0, labels.Length).Select(i => (double)i).ToArray(),
            labels
        );

        plot.Axes.Bottom.Label.Text = "Cantidad de Misiones";

        // Añadir margen derecho para las etiquetas
        plot.Axes.Margins(right: 0.15);

        // Estilo
        //plot.Title("Misiones Insatisfactorias por Aeronave");
        plot.FigureBackground.Color = Colors.White;
        plot.DataBackground.Color = Colors.White;

        // Tamaño
        plot.ScaleFactor = 2;

        return plot.GetImage(ChartWidth, ChartHeight).GetImageBytes();
    }

    public byte[] GenerateInstructorUnsatisfactoryChart(List<InstructorUnsatisfactoryMissionsDto> data)
    {
        if (data.Count == 0)
            return GenerateEmptyChart("Sin datos de misiones insatisfactorias por instructor");

        var plot = new Plot();

        double[] values = data.Select(x => (double)x.UnsatisfactoryCount).ToArray();
        string[] labels = data.Select(x => TruncateText(x.InstructorName, 20)).ToArray();

        var bars = plot.Add.Bars(values);
        bars.Horizontal = true;

        // Aplicar colores del degradado naranja
        for (int i = 0; i < bars.Bars.Count; i++)
        {
            bars.Bars[i].FillColor = InstitutionalColors.OrangeGradient[i % InstitutionalColors.OrangeGradient.Length];
        }

        // Agregar etiquetas con valores
        for (int i = 0; i < values.Length; i++)
        {
            var text = plot.Add.Text(values[i].ToString(), values[i], i);
            text.LabelFontSize = 10;
            text.LabelBold = true;
            text.LabelFontColor = Colors.Black;
            text.OffsetX = 10;
            text.LabelAlignment = Alignment.MiddleLeft;
        }

        // Configurar ejes
        plot.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
            Enumerable.Range(0, labels.Length).Select(i => (double)i).ToArray(),
            labels
        );

        plot.Axes.Bottom.Label.Text = "Cantidad de Misiones";

        // Añadir margen derecho para las etiquetas (25% extra)
        plot.Axes.Margins(right: 0.15);

        // Estilo
        //plot.Title("Misiones Insatisfactorias por Instructor");
        plot.FigureBackground.Color = Colors.White;
        plot.DataBackground.Color = Colors.White;

        // Tamaño
        plot.ScaleFactor = 2;

        return plot.GetImage(ChartWidth, ChartHeight).GetImageBytes();
    }

    public byte[] GenerateNRedCategoriesChart(NRedReasonsDto data)
    {
        if (data.Categories.Count == 0)
            return GenerateEmptyChart("Sin datos de categorías N ROJA");

        var plot = new Plot();

        double[] values = data.Categories.Select(x => (double)x.Count).ToArray();
        // Remover la palabra "FACTOR" de las etiquetas y limpiar espacios
        string[] labels = data.Categories
            .Select(x => x.Category.Replace("FACTOR", "", StringComparison.OrdinalIgnoreCase).Trim())
            .ToArray();

        var bars = plot.Add.Bars(values);

        // Aplicar color naranja
        foreach (var bar in bars.Bars)
        {
            bar.FillColor = InstitutionalColors.OrangeLight;
        }

        // Agregar etiquetas con valores
        for (int i = 0; i < values.Length; i++)
        {
            var text = plot.Add.Text(values[i].ToString(), i, values[i]);
            text.LabelFontSize = 11;
            text.LabelBold = true;
            text.LabelFontColor = Colors.Black;
            text.OffsetY = 0;
            text.LabelAlignment = Alignment.LowerCenter;
        }

        // Configurar ejes
        plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
            Enumerable.Range(0, labels.Length).Select(i => (double)i).ToArray(),
            labels
        );

        // Etiquetas horizontales para mejor legibilidad
        plot.Axes.Bottom.TickLabelStyle.Rotation = 0;

        plot.Axes.Left.Label.Text = "Cantidad";
        plot.Axes.Bottom.Label.Text = "Categoría";

        // Añadir margen superior para las etiquetas (20% extra)
        plot.Axes.Margins(top: 0.20);

        // Estilo
        //plot.Title("Categorías N ROJA");
        plot.FigureBackground.Color = Colors.White;
        plot.DataBackground.Color = Colors.White;

        // Tamaño
        plot.ScaleFactor = 2;

        return plot.GetImage(ChartWidth, ChartHeight).GetImageBytes();
    }

    // Helper methods
    private static byte[] GenerateEmptyChart(string message)
    {
        var plot = new Plot();

        // Configurar límites de ejes primero (0 a 1)
        plot.Axes.SetLimits(0, 1, 0, 1);

        // Añadir texto centrado usando coordenadas relativas
        var text = plot.Add.Text(message, 0.5, 0.5);
        text.LabelAlignment = Alignment.MiddleCenter;
        text.LabelFontSize = 14;
        text.LabelFontColor = Colors.Gray;

        plot.HideGrid();
        plot.Axes.Frameless();

        plot.FigureBackground.Color = Colors.White;
        plot.DataBackground.Color = Colors.White;

        plot.ScaleFactor = 2;

        return plot.GetImage(ChartWidth, ChartHeight).GetImageBytes();
    }

    private static string TruncateText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
            return text;

        return text[..(maxLength - 3)] + "...";
    }
}