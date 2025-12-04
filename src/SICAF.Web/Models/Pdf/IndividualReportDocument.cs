using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using SICAF.Common.Constants;
using SICAF.Common.DTOs.Reports;

namespace SICAF.Web.Models.Pdf;

public class IndividualReportDocument : IDocument
{
    private readonly IndividualReportDto _data;
    private readonly byte[] _logo;
    private readonly byte[] _studentPhoto;

    private static class Colors
    {
        public static string GreenPrimary => "#409448";
        public static string White => "#FFFFFF";
        public static string LightGray => "#F5F5F5";
        public static string Gray => "#666666";
        public static string Black => "#000000";
        public static string Blue => "#5DADE2";
        public static string Red => "#D0372F";
        public static string Orange => "#F87C63";
    }

    public IndividualReportDocument(IndividualReportDto data, byte[] logo, byte[] studentPhoto)
    {
        _data = data;
        _logo = logo;
        _studentPhoto = studentPhoto;
    }

    public DocumentMetadata GetMetadata() => new DocumentMetadata
    {
        Title = $"Informe Académico Individual - {_data.FullName}",
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
                    column.Item().AlignRight().PaddingTop(5).Text("INFORME ACADÉMICO INDIVIDUAL").FontSize(16).Bold();
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

            // Información del Estudiante
            column.Item().Element(ComposeStudentInfo);

            // Desempeño Académico
            column.Item().Element(ComposeAcademicPerformance);

            // Progreso del programa
            column.Item().Element(ComposeCourseProgress);

            // Misiones Insatisfactorias
            column.Item().Element(ComposeUnsatisfactoryMissions);

            // Comités Académicos
            column.Item().Element(ComposeAcademicCommittees);
        });
    }

    private void ComposeStudentInfo(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("Información del Estudiante").FontSize(14).Bold().FontColor(Colors.GreenPrimary);

            column.Item().PaddingTop(5).Border(1).BorderColor(Colors.GreenPrimary).Padding(10).Row(row =>
            {
                // Foto del estudiante (izquierda)
                row.ConstantItem(120).Height(100).Image(_studentPhoto);

                // Información del estudiante (derecha)
                row.RelativeItem().PaddingLeft(15).Column(info =>
                {
                    info.Item().Text(txt =>
                    {
                        txt.Span(_data.Grade).Bold().FontSize(12);
                        txt.Span(". ").FontSize(12);
                        txt.Span(_data.FullName).Bold().FontSize(12);
                    });

                    info.Item().PaddingTop(5).Row(r =>
                    {
                        r.RelativeItem().Text(txt =>
                        {
                            txt.Span($"{_data.IdentificationType}: ").FontSize(9).FontColor(Colors.Gray);
                            txt.Span(_data.IdentificationNumber).FontSize(9);
                        });

                        r.RelativeItem().Text(txt =>
                        {
                            txt.Span("PID: ").FontSize(9).FontColor(Colors.Gray);
                            txt.Span(_data.PID).FontSize(9);
                        });
                    });

                    info.Item().PaddingTop(5).Row(r =>
                    {
                        r.RelativeItem().Text(txt =>
                        {
                            txt.Span("Ala: ").FontSize(9).FontColor(Colors.Gray);
                            txt.Span(_data.WingType).FontSize(9);
                        });

                        r.RelativeItem().Text(txt =>
                        {
                            txt.Span("Fuerza: ").FontSize(9).FontColor(Colors.Gray);
                            txt.Span(_data.Force).FontSize(9);
                        });
                    });

                    info.Item().PaddingTop(5).Row(r =>
                    {
                        r.RelativeItem().Text(txt =>
                        {
                            txt.Span("Estado Programa: ").FontSize(9).FontColor(Colors.Gray);
                            txt.Span(_data.StudentStatusDisplay).FontSize(9).Bold()
                                .FontColor(GetStatusColor(_data.StudentStatus));
                        });

                        if (!string.IsNullOrEmpty(_data.PhaseStatus))
                        {
                            r.RelativeItem().Text(txt =>
                            {
                                txt.Span("Estado Fase: ").FontSize(9).FontColor(Colors.Gray);
                                txt.Span(_data.PhaseStatus).FontSize(9).Bold()
                                    .FontColor(GetStatusColor(_data.PhaseStatus));
                            });
                        }
                    });

                    if (!string.IsNullOrEmpty(_data.PhaseName))
                    {
                        info.Item().PaddingTop(5).Text(txt =>
                        {
                            txt.Span("Fase Filtrada: ").FontSize(9).FontColor(Colors.Gray);
                            txt.Span(_data.PhaseName).FontSize(9).Bold();
                        });
                    }
                });
            });
        });
    }

    private void ComposeAcademicPerformance(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("Desempeño Académico").FontSize(14).Bold().FontColor(Colors.GreenPrimary);

            column.Item().PaddingTop(5).Row(row =>
            {
                row.Spacing(8);

                // Calificación A
                row.RelativeItem().Border(1).BorderColor(Colors.GreenPrimary).Padding(8).Column(stat =>
                {
                    stat.Item().AlignCenter().Text("Calificación A").FontSize(8).FontColor(Colors.Gray);
                    stat.Item().AlignCenter().Text(_data.GradeDistribution.GradeA.ToString()).FontSize(16).Bold().FontColor(Colors.GreenPrimary);
                });

                // Calificación B
                row.RelativeItem().Border(1).BorderColor(Colors.Blue).Padding(8).Column(stat =>
                {
                    stat.Item().AlignCenter().Text("Calificación B").FontSize(8).FontColor(Colors.Gray);
                    stat.Item().AlignCenter().Text(_data.GradeDistribution.GradeB.ToString()).FontSize(16).Bold().FontColor(Colors.Blue);
                });

                // Calificación C
                row.RelativeItem().Border(1).BorderColor(Colors.GreenPrimary).Padding(8).Column(stat =>
                {
                    stat.Item().AlignCenter().Text("Calificación C").FontSize(8).FontColor(Colors.Gray);
                    stat.Item().AlignCenter().Text(_data.GradeDistribution.GradeC.ToString()).FontSize(16).Bold().FontColor(Colors.GreenPrimary);
                });

                // Calificación N
                row.RelativeItem().Border(1).BorderColor(Colors.Orange).Padding(8).Column(stat =>
                {
                    stat.Item().AlignCenter().Text("Calificación N").FontSize(8).FontColor(Colors.Gray);
                    stat.Item().AlignCenter().Text(_data.GradeDistribution.GradeN.ToString()).FontSize(16).Bold().FontColor(Colors.Orange);
                });

                // Calificación N ROJA
                row.RelativeItem().Border(1).BorderColor(Colors.Red).Padding(8).Column(stat =>
                {
                    stat.Item().AlignCenter().Text("Calificación N ROJA").FontSize(8).FontColor(Colors.Gray);
                    stat.Item().AlignCenter().Text(_data.GradeDistribution.GradeNR.ToString()).FontSize(16).Bold().FontColor(Colors.Red);
                });

                // Promedio
                row.RelativeItem().Border(1).BorderColor(Colors.GreenPrimary).Padding(8).Column(stat =>
                {
                    stat.Item().AlignCenter().Text("Promedio Académico").FontSize(8).FontColor(Colors.Gray);
                    stat.Item().AlignCenter().Text($"{_data.Average:F2}").FontSize(16).Bold().FontColor(Colors.GreenPrimary);
                });
            });
        });
    }

    private void ComposeCourseProgress(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("Progreso del Programa").FontSize(14).Bold().FontColor(Colors.GreenPrimary);

            column.Item().PaddingTop(5).Row(row =>
            {
                row.Spacing(10);

                // Horas de Vuelo
                row.RelativeItem().Border(1).BorderColor(Colors.GreenPrimary).Padding(10).Column(stat =>
                {
                    stat.Item().Text("Horas de Vuelo").FontSize(9).FontColor(Colors.Gray);
                    stat.Item().PaddingTop(3).Text($"{_data.TotalFlightHours:F2}").FontSize(16).Bold();
                });

                // Fase Actual
                row.RelativeItem().Border(1).BorderColor(Colors.GreenPrimary).Padding(10).Column(stat =>
                {
                    stat.Item().Text("Fase Actual").FontSize(9).FontColor(Colors.Gray);
                    if (!string.IsNullOrEmpty(_data.CurrentPhase))
                    {
                        stat.Item().PaddingTop(3).Text(_data.CurrentPhase).FontSize(11).Bold();
                        stat.Item().PaddingTop(2).Text($"{_data.CurrentPhaseMissionsCompleted}/{_data.CurrentPhaseTotalMissions} misiones").FontSize(8);
                    }
                    else
                    {
                        stat.Item().PaddingTop(3).Text(GetCurrentPhaseText()).FontSize(9).Italic();
                    }
                });

                // Misiones No Evaluables
                row.RelativeItem().Border(1).BorderColor(Colors.GreenPrimary).Padding(10).Column(stat =>
                {
                    stat.Item().Text("Misiones No Evaluables").FontSize(9).FontColor(Colors.Gray);
                    stat.Item().PaddingTop(3).Text(_data.NonEvaluableMissionRecords.ToString()).FontSize(16).Bold();
                });

                // Fases Completadas
                row.RelativeItem().Border(1).BorderColor(Colors.GreenPrimary).Padding(10).Column(stat =>
                {
                    stat.Item().Text("Fases Completadas").FontSize(9).FontColor(Colors.Gray);
                    stat.Item().PaddingTop(3).Text(_data.PhasesCompleted.ToString()).FontSize(16).Bold();
                });
            });

            // Progreso Total
            column.Item().PaddingTop(10).Border(1).BorderColor(Colors.Blue).Padding(10).Column(progress =>
            {
                progress.Item().Text("Progreso Total del Programa").FontSize(11).Bold();
                progress.Item().PaddingTop(5).Text($"{_data.TotalMissionsCompleted} / {_data.TotalMissionsInCourse} misiones").FontSize(10);

                progress.Item().PaddingTop(5).Row(progressBar =>
                {
                    var progressPercentage = _data.TotalCourseProgressPercentage;

                    progressBar.RelativeItem((float)progressPercentage).Height(25)
                        .Background(Colors.Blue).AlignMiddle().PaddingHorizontal(5)
                        .Text($"{progressPercentage:F2}%").FontColor(Colors.White).FontSize(10).Bold();

                    if (progressPercentage < 100)
                    {
                        progressBar.RelativeItem((float)(100 - progressPercentage)).Height(25)
                            .Background(Colors.LightGray);
                    }
                });
            });
        });
    }

    private void ComposeUnsatisfactoryMissions(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("Misiones Insatisfactorias").FontSize(14).Bold().FontColor(Colors.GreenPrimary);

            if (_data.UnsatisfactoryMissions.Count > 0)
            {
                column.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1);  // Fase
                        columns.RelativeColumn(1);  // Misión
                        columns.RelativeColumn(2);  // Tareas con N Roja
                        columns.RelativeColumn(1);  // Fecha
                        columns.RelativeColumn(2);  // Instructor
                        columns.RelativeColumn(2);  // Observaciones
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.GreenPrimary).Padding(5).Text("Fase").FontColor(Colors.White).FontSize(8).Bold();
                        header.Cell().Background(Colors.GreenPrimary).Padding(5).Text("Misión").FontColor(Colors.White).FontSize(8).Bold();
                        header.Cell().Background(Colors.GreenPrimary).Padding(5).Text("Tareas con N Roja").FontColor(Colors.White).FontSize(8).Bold();
                        header.Cell().Background(Colors.GreenPrimary).Padding(5).Text("Fecha").FontColor(Colors.White).FontSize(8).Bold();
                        header.Cell().Background(Colors.GreenPrimary).Padding(5).Text("Instructor").FontColor(Colors.White).FontSize(8).Bold();
                        header.Cell().Background(Colors.GreenPrimary).Padding(5).Text("Observaciones").FontColor(Colors.White).FontSize(8).Bold();
                    });

                    // Body
                    foreach (var mission in _data.UnsatisfactoryMissions)
                    {
                        table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).Text(mission.Phase).FontSize(8);
                        table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).Text(mission.Mission).FontSize(8);
                        table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).Column(tasksCol =>
                        {
                            foreach (var task in mission.TasksWithNRed)
                            {
                                tasksCol.Item().Text($"• {task}").FontSize(7);
                            }
                        });
                        table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).Text(mission.Date.ToString("dd/MM/yyyy")).FontSize(8);
                        table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).Text(mission.InstructorGradeAndName).FontSize(8);
                        table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).Text(mission.Observations).FontSize(7);
                    }
                });
            }
            else
            {
                column.Item().PaddingTop(5).Background("#E8F5E9").Padding(15).AlignCenter().Column(msg =>
                {
                    msg.Item().Text("✓").FontSize(24).FontColor(Colors.GreenPrimary);
                    msg.Item().PaddingTop(5).Text("¡Excelente! No hay misiones insatisfactorias registradas.").FontSize(10).Bold();
                });
            }
        });
    }

    private void ComposeAcademicCommittees(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("Comités Académicos").FontSize(14).Bold().FontColor(Colors.GreenPrimary);

            if (_data.AcademicCommittees.Count > 0)
            {
                column.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1);  // Número de Acta
                        columns.RelativeColumn(1);  // Fecha
                        columns.RelativeColumn(2);  // Decisión
                        columns.RelativeColumn(3);  // Observaciones
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.GreenPrimary).Padding(5).Text("Número de Acta").FontColor(Colors.White).FontSize(9).Bold();
                        header.Cell().Background(Colors.GreenPrimary).Padding(5).Text("Fecha").FontColor(Colors.White).FontSize(9).Bold();
                        header.Cell().Background(Colors.GreenPrimary).Padding(5).Text("Decisión").FontColor(Colors.White).FontSize(9).Bold();
                        header.Cell().Background(Colors.GreenPrimary).Padding(5).Text("Observaciones").FontColor(Colors.White).FontSize(9).Bold();
                    });

                    // Body
                    foreach (var committee in _data.AcademicCommittees)
                    {
                        table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).Text(committee.ActNumber).FontSize(9);
                        table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).Text(committee.Date.ToString("dd/MM/yyyy")).FontSize(9);
                        table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).Text(committee.Decision).FontSize(9)
                            .FontColor(GetCommitteeDecisionColor(committee.Decision)).Bold();
                        table.Cell().Border(0.5f).BorderColor(Colors.Gray).Padding(5).Text(committee.Observations).FontSize(8);
                    }
                });
            }
            else
            {
                column.Item().PaddingTop(5).Background("#E3F2FD").Padding(15).AlignCenter().Column(msg =>
                {
                    msg.Item().PaddingTop(5).Text("El estudiante no ha sido convocado a comités académicos.").FontSize(10);
                });
            }
        });
    }

    // Helper methods
    private static string GetStatusColor(string status)
    {
        return status switch
        {
            UserConstants.StudentStatus.Active => Colors.Blue,
            UserConstants.StudentStatus.PhaseCompleted => Colors.GreenPrimary,
            UserConstants.StudentStatus.Suspended => Colors.Red,
            UserConstants.StudentStatus.CourseCompleted => Colors.Blue,
            _ => Colors.Gray
        };
    }

    private static string GetCommitteeDecisionColor(string decision)
    {
        return decision switch
        {
            UserConstants.CommitteeDecisions.ContinueCourse => Colors.GreenPrimary,
            UserConstants.CommitteeDecisions.Suspendecourse => Colors.Red,
            _ => Colors.Gray
        };
    }

    private string GetCurrentPhaseText()
    {
        return _data.StudentStatus switch
        {
            UserConstants.StudentStatus.CourseCompleted => "Programa Completado",
            UserConstants.StudentStatus.Suspended => "Estudiante Suspendido",
            _ => "Sin fase asignada"
        };
    }
}
