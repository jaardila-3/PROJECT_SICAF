// ===== Funciones para inicializar gráficas ApexCharts =====

function initializeGradeDistributionChart(gradeDistribution, reportInfo) {
    // Array para mostrar en la gráfica
    const gradeLabels = ['A', 'B', 'C', 'N', 'N ROJA'];
    // Array para los valores de backend/filtros
    const gradeValues = ['A', 'B', 'C', 'N', 'NR'];

    // ===== Chart 1: Grade Distribution (Vertical Bars) =====
    var gradeDistributionOptions = {
        series: [{
            name: 'Cantidad',
            data: [
                gradeDistribution.gradeA,
                gradeDistribution.gradeB,
                gradeDistribution.gradeC,
                gradeDistribution.gradeN,
                gradeDistribution.gradeNR
            ]
        }],
        chart: {
            type: 'bar',
            height: 350,
            toolbar: { show: true },
            events: {
                dataPointSelection: function (event, chartContext, config) {
                    // Usa gradeValues para el filtro del backend
                    const selectedGrade = gradeValues[config.dataPointIndex];

                    const url = '/Reports/Report/ReportDetail' +
                        `?type=grades&value=${selectedGrade}` +
                        `&courseId=${reportInfo.courseId}` +
                        `&force=${reportInfo.force}` +
                        `&wingType=${reportInfo.wingType}` +
                        `&phaseId=${reportInfo.phaseId}`;

                    window.open(url, '_blank');
                }
            }
        },
        plotOptions: {
            bar: {
                borderRadius: 4,
                dataLabels: { position: 'top' },
                distributed: true // activa colores individuales por barra
            }
        },
        legend: {
            show: false // oculta la leyenda inferior generada por la propiedad distributed de plotOptions bar
        },
        dataLabels: {
            enabled: true,
            offsetY: -20,
            style: {
                fontSize: '12px',
                colors: ["#304758"]
            }
        },
        xaxis: {
            categories: gradeLabels,
            position: 'bottom'
        },
        yaxis: {
            title: { text: 'Cantidad' }
        },
        colors: ['#409448', '#4DB357', '#6DC075', '#F87C63', '#D0372F'],
        title: {
            text: 'Distribución de Calificaciones',
            align: 'center'
        }
    };
    var gradeDistributionChart = new ApexCharts(
        document.querySelector("#gradeDistributionChart"),
        gradeDistributionOptions
    );
    gradeDistributionChart.render();
}

function initializeMachineFlightHoursChart(machineFlightHours, reportInfo) {
    // ===== Chart 2: Machine Flight Hours (Horizontal Bars) =====
    var machineFlightHoursOptions = {
        series: [{
            name: 'Horas de Vuelo',
            data: machineFlightHours.map(m => m.totalHours)
        }],
        chart: {
            type: 'bar',
            height: 350,
            toolbar: { show: true },
            events: {
                dataPointSelection: function (event, chartContext, config) {

                    const selectedAircraft = machineFlightHours[config.dataPointIndex].aircraftRegistration;

                    const url = '/Reports/Report/ReportDetail' +
                        `?type=machine-hours&value=${selectedAircraft}` +
                        `&courseId=${reportInfo.courseId}` +
                        `&force=${reportInfo.force}` +
                        `&wingType=${reportInfo.wingType}` +
                        `&phaseId=${reportInfo.phaseId}`;

                    window.open(url, '_blank');
                }
            }
        },
        plotOptions: {
            bar: {
                borderRadius: 4,
                horizontal: true,
                distributed: true // activa colores individuales por barra
                //dataLabels: { position: 'top' }
            }
        },
        legend: {
            show: false // oculta la leyenda inferior generada por la propiedad distributed de plotOptions bar
        },
        dataLabels: {
            enabled: true,
            formatter: function (val) {
                return val.toFixed(2) + " hrs";
            },
            offsetX: 20,
            style: {
                fontSize: '12px',
                colors: ["#304758"]
            }
        },
        xaxis: {
            categories: machineFlightHours.map(m => m.aircraftRegistration),
            title: { text: 'Horas de Vuelo' }
        },
        colors: ['#409448', '#4DB357', '#6DC075', '#7BCB62', '#B5E2A7', '#62CBB8', '#4AC27E'],
        title: {
            text: 'Horas Vuelo por Aeronave',
            align: 'center'
        }
    };
    var machineFlightHoursChart = new ApexCharts(
        document.querySelector("#machineFlightHoursChart"),
        machineFlightHoursOptions
    );
    machineFlightHoursChart.render();
}

function initializeInstructorFlightHoursChart(instructorFlightHours, reportInfo) {
    // ===== Chart 3: Instructor Flight Hours (Horizontal Bars) =====
    var instructorFlightHoursOptions = {
        series: [{
            name: 'Horas de Vuelo',
            data: instructorFlightHours.map(i => i.totalHours)
        }],
        chart: {
            type: 'bar',
            height: 350,
            toolbar: { show: true },
            events: {
                dataPointSelection: function (event, chartContext, config) {

                    const selectedInstructor = instructorFlightHours[config.dataPointIndex].instructorName;

                    const url = '/Reports/Report/ReportDetail' +
                        `?type=instructor-hours&value=${encodeURIComponent(selectedInstructor)}` +
                        `&courseId=${reportInfo.courseId}` +
                        `&force=${reportInfo.force}` +
                        `&wingType=${reportInfo.wingType}` +
                        `&phaseId=${reportInfo.phaseId}`;

                    window.open(url, '_blank');
                }
            }
        },
        plotOptions: {
            bar: {
                borderRadius: 4,
                horizontal: true,
                distributed: true // activa colores individuales por barra
                //dataLabels: { position: 'top' }
            }
        },
        legend: {
            show: false // oculta la leyenda inferior generada por la propiedad distributed de plotOptions bar
        },
        dataLabels: {
            enabled: true,
            formatter: function (val, opts) {
                const instructorName = opts.w.globals.labels[opts.dataPointIndex];
                return `${instructorName}: ${val.toFixed(2)} hrs`;
            },
            offsetX: 10,
            style: {
                fontSize: '12px',
                colors: ["#304758"]
            }
        },
        xaxis: {
            categories: instructorFlightHours.map(i => i.instructorName),
            title: { text: 'Horas de Vuelo' }
        },
        yaxis: {
            labels: {
                show: false // oculta los nombres de instructores en el eje Y
            }
        },
        colors: ['#409448', '#4DB357', '#6DC075', '#7BCB62', '#B5E2A7', '#62CBB8', '#4AC27E'],
        title: {
            text: 'Horas Vuelo Instructor',
            align: 'center'
        }
    };
    var instructorFlightHoursChart = new ApexCharts(
        document.querySelector("#instructorFlightHoursChart"),
        instructorFlightHoursOptions
    );
    instructorFlightHoursChart.render();
}

function initializeMachineUnsatisfactoryChart(machineUnsatisfactory, reportInfo) {
    // ===== Chart 4: Machine Unsatisfactory Missions (Horizontal Bars) =====
    var machineUnsatisfactoryOptions = {
        series: [{
            name: 'Misiones Ins..',
            data: machineUnsatisfactory.map(m => m.unsatisfactoryCount)
        }],
        chart: {
            type: 'bar',
            height: 350,
            toolbar: { show: true },
            events: {
                dataPointSelection: function (event, chartContext, config) {

                    const selectedAircraft = machineUnsatisfactory[config.dataPointIndex].aircraftRegistration;

                    const url = '/Reports/Report/ReportDetail' +
                        `?type=machine-unsatisfactory&value=${encodeURIComponent(selectedAircraft)}` +
                        `&courseId=${reportInfo.courseId}` +
                        `&force=${reportInfo.force}` +
                        `&wingType=${reportInfo.wingType}` +
                        `&phaseId=${reportInfo.phaseId}`;

                    window.open(url, '_blank');
                }
            }
        },
        plotOptions: {
            bar: {
                borderRadius: 4,
                horizontal: true,
                //dataLabels: { position: 'top' }
            }
        },
        dataLabels: {
            enabled: true,
            offsetX: 20,
            style: {
                fontSize: '12px',
                colors: ["#304758"]
            }
        },
        xaxis: {
            categories: machineUnsatisfactory.map(m => m.aircraftRegistration),
            title: { text: 'Cantidad de Misiones' }
        },
        colors: ['#F87C63'],
        title: {
            text: 'Insatisfactorias por Aeronave',
            align: 'center'
        }
    };
    var machineUnsatisfactoryChart = new ApexCharts(
        document.querySelector("#machineUnsatisfactoryChart"),
        machineUnsatisfactoryOptions
    );
    machineUnsatisfactoryChart.render();
}

function initializeInstructorUnsatisfactoryChart(instructorUnsatisfactory, reportInfo) {
    // ===== Chart 5: Instructor Unsatisfactory Missions (Horizontal Bars) =====
    var instructorUnsatisfactoryOptions = {
        series: [{
            name: 'Misiones Ins..',
            data: instructorUnsatisfactory.map(i => i.unsatisfactoryCount)
        }],
        chart: {
            type: 'bar',
            height: 350,
            toolbar: { show: true },
            events: {
                dataPointSelection: function (event, chartContext, config) {

                    const selectedInstructor = instructorUnsatisfactory[config.dataPointIndex].instructorName;

                    const url = '/Reports/Report/ReportDetail' +
                        `?type=instructor-unsatisfactory&value=${encodeURIComponent(selectedInstructor)}` +
                        `&courseId=${reportInfo.courseId}` +
                        `&force=${reportInfo.force}` +
                        `&wingType=${reportInfo.wingType}` +
                        `&phaseId=${reportInfo.phaseId}`;

                    window.open(url, '_blank');
                }
            }
        },
        plotOptions: {
            bar: {
                borderRadius: 4,
                horizontal: true,
                distributed: true // activa colores individuales por barra
                //dataLabels: { position: 'top' }
            }
        },
        legend: {
            show: false // oculta la leyenda inferior generada por la propiedad distributed de plotOptions bar
        },
        dataLabels: {
            enabled: true,
            offsetX: 10,
            formatter: function (val, opts) {
                const instructorName = opts.w.globals.labels[opts.dataPointIndex];
                return `${instructorName}: ${val.toFixed(2)} hrs`;
            },
            style: {
                fontSize: '12px',
                colors: ["#304758"]
            }
        },
        xaxis: {
            categories: instructorUnsatisfactory.map(i => i.instructorName),
            title: { text: 'Cantidad de Misiones' }
        },
        yaxis: {
            labels: {
                show: false // oculta los nombres de instructores en el eje Y
            }
        },
        colors: ['#F87C63', '#C2744A', '#D6A185'],
        title: {
            text: 'Insatisfactorias por Instructor',
            align: 'center'
        }
    };
    var instructorUnsatisfactoryChart = new ApexCharts(
        document.querySelector("#instructorUnsatisfactoryChart"),
        instructorUnsatisfactoryOptions
    );
    instructorUnsatisfactoryChart.render();
}

function initializeNRedCategoriesChart(nRedReasons, reportInfo) {
    // ===== Chart 6: NR Categories (Vertical Bars) =====
    var nRedCategoriesOptions = {
        series: [{
            name: 'Cantidad',
            data: nRedReasons.categories.map(c => c.count)
        }],
        chart: {
            type: 'bar',
            height: 350,
            toolbar: { show: true },
            events: {
                dataPointSelection: function (event, chartContext, config) {

                    const selectedCategory = nRedReasons.categories[config.dataPointIndex].category;

                    const url = '/Reports/Report/ReportDetail' +
                        `?type=nred-categories&value=${encodeURIComponent(selectedCategory)}` +
                        `&courseId=${reportInfo.courseId}` +
                        `&force=${reportInfo.force}` +
                        `&wingType=${reportInfo.wingType}` +
                        `&phaseId=${reportInfo.phaseId}`;

                    window.open(url, '_blank');
                }
            }
        },
        plotOptions: {
            bar: {
                borderRadius: 4,
                dataLabels: { position: 'top' }
            }
        },
        dataLabels: {
            enabled: true,
            offsetY: -20,
            style: {
                fontSize: '12px',
                colors: ["#304758"]
            }
        },
        xaxis: {
            categories: nRedReasons.categories.map(c => c.category),
            labels: {
                rotate: -45,
                rotateAlways: true
            }
        },
        yaxis: {
            title: { text: 'Cantidad' }
        },
        colors: ['#F87C63'],
        title: {
            text: 'Categorías N ROJA',
            align: 'center'
        }
    };
    var nRedCategoriesChart = new ApexCharts(
        document.querySelector("#nRedCategoriesChart"),
        nRedCategoriesOptions
    );
    nRedCategoriesChart.render();
}

// Función principal para inicializar todas las gráficas
function initializeAllCharts(data, reportInfo) {
    initializeGradeDistributionChart(data.gradeDistribution, reportInfo);
    initializeMachineFlightHoursChart(data.machineFlightHours, reportInfo);
    initializeInstructorFlightHoursChart(data.instructorFlightHours, reportInfo);
    initializeMachineUnsatisfactoryChart(data.machineUnsatisfactory, reportInfo);
    initializeInstructorUnsatisfactoryChart(data.instructorUnsatisfactory, reportInfo);
    initializeNRedCategoriesChart(data.nRedReasons, reportInfo);
}