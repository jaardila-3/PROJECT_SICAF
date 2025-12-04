using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using SICAF.Common.DTOs.Reports;

namespace SICAF.Web.Models.Pdf;

public class GeneralReportDocument : IDocument
{
    private readonly GeneralReportDto _data;
    private readonly byte[] _logo;
    private readonly Dictionary<string, byte[]> _charts;

    private static class Colors
    {
        public static string GreenPrimary => "#409448";
        public static string White => "#FFFFFF";
        public static string LightGray => "#F5F5F5";
        public static string Gray => "#666666";
        public static string Black => "#000000";
    }

    public GeneralReportDocument(GeneralReportDto data, byte[] logo, Dictionary<string, byte[]> charts)
    {
        _data = data;
        _logo = logo;
        _charts = charts;
    }

    public DocumentMetadata GetMetadata() => new DocumentMetadata
    {
        Title = "Informe Académico General",
        Author = "Sistema SICAF - Escuela de Aviación Policial",
        Creator = "QuestPDF"
    };

    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(2, Unit.Centimetre);
            page.PageColor(QuestPDF.Helpers.Colors.White);
            page.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Black));

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Column(headerColumn =>
        {
            headerColumn.Item().Row(row =>
            {
                // Logo (izquierda)
                row.ConstantItem(80).Height(60).Image(_logo);

                // Texto institucional (derecha)
                row.RelativeItem().PaddingLeft(10).AlignMiddle().Column(column =>
                {
                    column.Item().AlignRight().Text("Escuela de Aviación Policial").FontSize(14).Bold().FontColor(Colors.GreenPrimary);
                    column.Item().AlignRight().Text("Policía Nacional de Colombia").FontSize(12).FontColor(Colors.Gray);
                    column.Item().AlignRight().PaddingTop(5).Text("INFORME ACADÉMICO GENERAL").FontSize(16).Bold();
                });
            });

            // Línea separadora
            headerColumn.Item().PaddingTop(5).BorderBottom(2).BorderColor(Colors.GreenPrimary);
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.Column(column =>
        {
            // Línea separadora
            column.Item().BorderTop(1).BorderColor(Colors.Gray);

            column.Item().PaddingTop(5).Row(row =>
            {
                row.RelativeItem().AlignLeft().Text(txt =>
                {
                    txt.Span("Fecha de generación: ").FontSize(8).FontColor(Colors.Gray);
                    txt.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(8);
                });

                row.RelativeItem().AlignCenter().Text(txt =>
                {
                    txt.Span("Página ").FontSize(8);
                    txt.CurrentPageNumber().FontSize(8);
                    txt.Span(" de ").FontSize(8);
                    txt.TotalPages().FontSize(8);
                });

                row.RelativeItem().AlignRight().Text("SICAF").FontSize(8).FontColor(Colors.Gray);
            });
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(15);

            // Información del programa
            column.Item().Element(ComposeCourseInfo);

            // Estadísticas Generales
            column.Item().Element(ComposeStatistics);

            // Tabla de Instructores
            if (_data.InstructorsTable.Count > 0)
            {
                column.Item().Element(ComposeInstructorsTable);
            }

            // Tabla de Estudiantes
            column.Item().Element(ComposeStudentsTable);

            // Gráficos
            column.Item().PageBreak();
            column.Item().Element(ComposeCharts);
        });
    }

    private void ComposeCourseInfo(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("Información del Programa").FontSize(14).Bold().FontColor(Colors.GreenPrimary);

            column.Item().PaddingTop(5).Background(Colors.LightGray).Padding(10).Column(info =>
            {
                info.Item().Row(row =>
                {
                    row.RelativeItem().Text(txt =>
                    {
                        txt.Span("Programa: ").Bold();
                        txt.Span(_data.CourseName);
                    });

                    row.RelativeItem().Text(txt =>
                    {
                        txt.Span("Fuerza: ").Bold();
                        txt.Span(_data.Force);
                    });
                });

                info.Item().PaddingTop(5).Row(row =>
                {
                    row.RelativeItem().Text(txt =>
                    {
                        txt.Span("Fecha Inicio: ").Bold();
                        txt.Span(_data.StartDate.ToString("dd/MM/yyyy"));
                    });

                    row.RelativeItem().Text(txt =>
                    {
                        txt.Span("Fecha Fin: ").Bold();
                        txt.Span(_data.EndDate.ToString("dd/MM/yyyy"));
                    });
                });

                info.Item().PaddingTop(5).Row(row =>
                {
                    row.RelativeItem().Text(txt =>
                    {
                        txt.Span("Tipo de Ala: ").Bold();
                        txt.Span(_data.WingType);
                    });

                    if (!string.IsNullOrEmpty(_data.PhaseName))
                    {
                        row.RelativeItem().Text(txt =>
                        {
                            txt.Span("Fase Filtrada: ").Bold();
                            txt.Span(_data.PhaseName);
                        });
                    }
                });
            });
        });
    }

    private void ComposeStatistics(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("Estadísticas Generales").FontSize(14).Bold().FontColor(Colors.GreenPrimary);

            column.Item().PaddingTop(5).Row(row =>
            {
                row.Spacing(10);

                row.RelativeItem().Border(1).BorderColor(Colors.GreenPrimary).Padding(10).Column(stat =>
                {
                    stat.Item().AlignCenter().Text("Total Estudiantes").FontSize(9).FontColor(Colors.Gray);
                    stat.Item().AlignCenter().Text(_data.TotalStudents.ToString()).FontSize(20).Bold().FontColor(Colors.GreenPrimary);
                });

                row.RelativeItem().Border(1).BorderColor(Colors.GreenPrimary).Padding(10).Column(stat =>
                {
                    stat.Item().AlignCenter().Text("Estudiantes Activos").FontSize(9).FontColor(Colors.Gray);
                    stat.Item().AlignCenter().Text(_data.ActiveStudents.ToString()).FontSize(20).Bold().FontColor(Colors.GreenPrimary);
                });

                row.RelativeItem().Border(1).BorderColor(Colors.GreenPrimary).Padding(10).Column(stat =>
                {
                    stat.Item().AlignCenter().Text("Estudiantes Suspendidos").FontSize(9).FontColor(Colors.Gray);
                    stat.Item().AlignCenter().Text(_data.SuspendedStudents.ToString()).FontSize(20).Bold().FontColor("#D0372F");
                });

                row.RelativeItem().Border(1).BorderColor(Colors.GreenPrimary).Padding(10).Column(stat =>
                {
                    stat.Item().AlignCenter().Text("Total Comités").FontSize(9).FontColor(Colors.Gray);
                    stat.Item().AlignCenter().Text(_data.TotalCommittees.ToString()).FontSize(20).Bold();
                });
            });
        });
    }

    private void ComposeInstructorsTable(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("Instructores y Líderes de Vuelo").FontSize(14).Bold().FontColor(Colors.GreenPrimary);

            column.Item().PaddingTop(5).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(40);  // No.
                    columns.RelativeColumn(3);   // Grado Nombres y Apellidos
                    columns.RelativeColumn(1);   // PID
                    columns.RelativeColumn(1);   // Ala
                    columns.RelativeColumn(2);   // Rol
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Background(Colors.GreenPrimary).Padding(5).AlignCenter().Text("No.").FontColor(Colors.White).FontSize(9);
                    header.Cell().Background(Colors.GreenPrimary).Padding(5).AlignCenter().Text("Grado Nombres y Apellidos").FontColor(Colors.White).FontSize(9);
                    header.Cell().Background(Colors.GreenPrimary).Padding(5).AlignCenter().Text("PID").FontColor(Colors.White).FontSize(9);
                    header.Cell().Background(Colors.GreenPrimary).Padding(5).AlignCenter().Text("Ala").FontColor(Colors.White).FontSize(9);
                    header.Cell().Background(Colors.GreenPrimary).Padding(5).AlignCenter().Text("Rol").FontColor(Colors.White).FontSize(9);
                });

                // Body
                foreach (var instructor in _data.InstructorsTable)
                {
                    table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).AlignCenter().Text(instructor.Number.ToString()).FontSize(9);
                    table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).Text($"{instructor.Grade}. {instructor.FullName}").FontSize(9);
                    table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).AlignCenter().Text(instructor.PID).FontSize(9);
                    table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).AlignCenter().Text(instructor.WingType).FontSize(9);
                    table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).Text(instructor.Role).FontSize(9);
                }
            });
        });
    }

    private void ComposeStudentsTable(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("Detalle de Estudiantes").FontSize(14).Bold().FontColor(Colors.GreenPrimary);

            column.Item().PaddingTop(5).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(4);   // Grado Nombres y Apellidos
                    columns.RelativeColumn(2);   // Identificación
                    columns.RelativeColumn(1);   // Misiones Completadas
                    columns.RelativeColumn(1);   // Progreso %
                    columns.RelativeColumn(1);   // Promedio Académico
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Background(Colors.GreenPrimary).Padding(5).Text("Grado Apellidos y Nombres").FontColor(Colors.White).FontSize(8);
                    header.Cell().Background(Colors.GreenPrimary).Padding(5).AlignCenter().Text("Identificación").FontColor(Colors.White).FontSize(9);
                    header.Cell().Background(Colors.GreenPrimary).Padding(5).AlignCenter().Text("Misiones Completas").FontColor(Colors.White).FontSize(9);
                    header.Cell().Background(Colors.GreenPrimary).Padding(5).AlignCenter().Text("Progreso (%)").FontColor(Colors.White).FontSize(9);
                    header.Cell().Background(Colors.GreenPrimary).Padding(5).AlignCenter().Text("Promedio Académico").FontColor(Colors.White).FontSize(9);
                });

                // Body
                foreach (var student in _data.Students)
                {
                    table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).Text(student.GradeAndName).FontSize(8);
                    table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).AlignCenter().Text(student.Identification).FontSize(8);
                    table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).AlignCenter().Text(student.CompletedMissions.ToString()).FontSize(8);
                    table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).AlignCenter().Text($"{student.ProgressPercentage:F2}%").FontSize(8);
                    table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).AlignCenter().Text($"{student.Average:F2}").FontSize(8);
                }
            });
        });
    }

    private void ComposeCharts(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("Análisis del Programa").FontSize(16).Bold().FontColor(Colors.GreenPrimary);

            column.Item().PaddingTop(20).Row(row =>
            {
                row.Spacing(10);

                // Gráfico 1: Distribución de Calificaciones
                row.RelativeItem().Column(col =>
                {
                    col.Item().AlignCenter().Text("Distribución de Calificaciones").FontSize(11).Bold();
                    col.Item().PaddingTop(5).Image(_charts["gradeDistribution"]);
                });

                // Gráfico 2: Horas de Vuelo por Aeronave
                row.RelativeItem().Column(col =>
                {
                    col.Item().AlignCenter().Text("Horas de Vuelo por Aeronave").FontSize(11).Bold();
                    col.Item().PaddingTop(5).Image(_charts["machineFlightHours"]);
                });
            });

            column.Item().PaddingTop(20).Row(row =>
            {
                row.Spacing(10);

                // Gráfico 3: Horas de Vuelo por Instructor
                row.RelativeItem().Column(col =>
                {
                    col.Item().AlignCenter().Text("Horas de Vuelo por Instructor").FontSize(11).Bold();
                    col.Item().PaddingTop(5).Image(_charts["instructorFlightHours"]);
                });

                // Gráfico 4: Misiones Insatisfactorias por Aeronave
                row.RelativeItem().Column(col =>
                {
                    col.Item().AlignCenter().Text("Misiones Insatisfactorias por Aeronave").FontSize(11).Bold();
                    col.Item().PaddingTop(5).Image(_charts["machineUnsatisfactory"]);
                });
            });

            //column.Item().PageBreak();

            column.Item().PaddingTop(20).Row(row =>
            {
                row.Spacing(10);

                // Gráfico 5: Misiones Insatisfactorias por Instructor
                row.RelativeItem().Column(col =>
                {
                    col.Item().AlignCenter().Text("Misiones Insatisfactorias por Instructor").FontSize(11).Bold();
                    col.Item().PaddingTop(5).Image(_charts["instructorUnsatisfactory"]);
                });

                // Gráfico 6: N ROJA por Categorías
                row.RelativeItem().Column(col =>
                {
                    col.Item().AlignCenter().Text("Categorías N ROJA").FontSize(11).Bold();
                    col.Item().PaddingTop(5).Image(_charts["nRedCategories"]);
                });
            });
        });
    }
}
