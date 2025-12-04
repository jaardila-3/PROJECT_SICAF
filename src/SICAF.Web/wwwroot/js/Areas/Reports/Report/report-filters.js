/**
 * @fileoverview JavaScript para manejar los selectores en cascada del módulo de informes
 * @description Maneja la lógica de habilitación/deshabilitación de selectores y botones
 * @requires jQuery, Select2, SweetAlert2
 */

(function ($) {
    'use strict';

    // Elementos del DOM
    const selectors = {
        course: $('#courseSelect'),
        force: $('#forceSelect'),
        wingType: $('#wingTypeSelect'),
        phase: $('#phaseSelect'),
        student: $('#studentSelect')
    };

    const forms = {
        general: $('#generalReportForm'),
        individual: $('#individualReportForm')
    };

    const hiddenInputs = {
        generalCourseId: $('#generalCourseId'),
        generalForce: $('#generalForce'),
        generalWingType: $('#generalWingType'),
        generalPhaseId: $('#generalPhaseId'),
        individualCourseId: $('#individualCourseId'),
        individualForce: $('#individualForce'),
        individualWingType: $('#individualWingType'),
        individualPhaseId: $('#individualPhaseId'),
        individualStudentId: $('#individualStudentId')
    };

    const noSelectionMessage = $('#noSelectionMessage');

    // Estado de filtros para evitar llamadas innecesarias a la API
    const filterState = {
        lastCourseId: null,
        lastForce: null,
        lastWingType: null,
        lastPhaseId: null,
        studentsLoaded: false,
        loadingInProgress: false  // Flag para evitar llamadas concurrentes
    };

    /**
     * Inicializa los event listeners
     */
    function initializeEventListeners() {
        selectors.course.on('change', handleCourseChange);
        selectors.force.on('change', handleForceChange);
        selectors.wingType.on('change', handleWingTypeChange);
        selectors.phase.on('change', handlePhaseChange);
        selectors.student.on('change', handleStudentChange);
    }

    /**
     * Maneja el cambio en el selector de programa
     */
    function handleCourseChange() {
        const courseId = $(this).val();

        if (!courseId) {
            resetFromForce();
            filterState.studentsLoaded = false;
            filterState.loadingInProgress = false;
            updateButtonsVisibility();
            return;
        }

        // Mostrar loading
        showLoadingState(selectors.force, 'Cargando fuerzas...');

        // Resetear selectores posteriores
        resetSelect(selectors.force);
        resetFromWingType();

        // Llamar a la API para obtener fuerzas
        fetchForces(courseId)
            .then(forces => {
                populateSelect(selectors.force, forces, 'Seleccione una fuerza...');
                enableSelect(selectors.force, 'Opcional - filtra estudiantes por fuerza');

                //  Cargar estudiantes del programa completo
                return loadStudentsIfNeeded(courseId, 'TODAS', 'TODAS', null);
            })
            .then(() => {
                updateButtonsVisibility();
            })
            .catch(error => {
                handleError('Error al cargar los datos', error);
                resetFromForce();
            });
    }

    /**
     * Maneja el cambio en el selector de fuerza
     */
    function handleForceChange() {
        const courseId = selectors.course.val();
        const force = $(this).val();

        if (!force) {
            resetFromWingType();
            // Recargar estudiantes del programa completo
            loadStudentsIfNeeded(courseId, 'TODAS', 'TODAS', null);
            return;
        }

        // Mostrar loading
        showLoadingState(selectors.wingType, 'Cargando tipos de ala...');

        // Resetear selectores posteriores
        resetSelect(selectors.wingType);
        resetSelect(selectors.phase);

        // Llamar a la API para obtener tipos de ala
        fetchWingTypes(courseId, force)
            .then(wingTypes => {
                populateSelect(selectors.wingType, wingTypes, 'Seleccione tipo de ala...');
                enableSelect(selectors.wingType, 'Opcional - filtra estudiantes por tipo de ala');

                //  Actualizar estudiantes filtrados por fuerza
                return loadStudentsIfNeeded(courseId, force, 'TODAS', null);
            })
            .then(() => {
                updateButtonsVisibility();
            })
            .catch(error => {
                handleError('Error al cargar los datos', error);
                resetFromWingType();
            });
    }

    /**
     * Maneja el cambio en el selector de tipo de ala
     */
    function handleWingTypeChange() {
        const courseId = selectors.course.val();
        const force = selectors.force.val();
        const wingType = $(this).val();

        if (!wingType) {
            resetFromPhase();
            // Recargar estudiantes con filtro actual
            loadStudentsIfNeeded(courseId, force || 'TODAS', 'TODAS', null);
            return;
        }

        // Mostrar loading
        showLoadingState(selectors.phase, 'Cargando fases...');

        // Resetear selectores posteriores
        resetSelect(selectors.phase);

        // Llamar a la API para obtener fases
        fetchPhases(wingType)
            .then(phases => {
                populateSelect(selectors.phase, phases, 'Seleccione una fase...');
                enableSelect(selectors.phase, 'Opcional - filtra estudiantes por fase');

                //  Actualizar estudiantes filtrados
                return loadStudentsIfNeeded(courseId, force || 'TODAS', wingType, null);
            })
            .then(() => {
                updateButtonsVisibility();
            })
            .catch(error => {
                handleError('Error al cargar las fases', error);
                resetFromPhase();
            });
    }

    /**
     * Maneja el cambio en el selector de fase
     */
    function handlePhaseChange() {
        const courseId = selectors.course.val();
        const force = selectors.force.val();
        const wingType = selectors.wingType.val();
        const phaseId = $(this).val();

        // Validar que existan los valores necesarios antes de cargar estudiantes
        if (!courseId || !force || !wingType) {
            return;
        }

        // Actualizar valores de los hidden inputs
        updateGeneralReportInputs();

        // Cargar estudiantes con filtro de fase
        loadStudentsIfNeeded(
            courseId,
            force || 'TODAS',
            wingType || 'TODAS',
            phaseId || null
        );
    }

    /**
     * Carga estudiantes solo si los filtros han cambiado significativamente
     * @param {string} courseId - ID del programa
     * @param {string} force - Fuerza seleccionada
     * @param {string} wingType - Tipo de ala seleccionado
     * @param {string} phaseId - ID de la fase (opcional)
     * @returns {Promise} - Promesa de la carga de estudiantes
     */
    function loadStudentsIfNeeded(courseId, force, wingType, phaseId) {
        // Normalizar valores vacíos a 'TODAS'
        const normalizedForce = force || 'TODAS';
        const normalizedWingType = wingType || 'TODAS';
        const normalizedPhaseId = phaseId || null;

        // Verificar si realmente necesitamos recargar
        const filtersChanged =
            filterState.lastCourseId !== courseId ||
            filterState.lastForce !== normalizedForce ||
            filterState.lastWingType !== normalizedWingType ||
            filterState.lastPhaseId !== normalizedPhaseId;

        // Verificar si el selector está vacío (sin opciones excepto el placeholder)
        const selectorIsEmpty = selectors.student.find('option').length <= 1;

        // Evitar llamadas concurrentes
        if (filterState.loadingInProgress && !filtersChanged) {
            return Promise.resolve();
        }

        if (!filtersChanged && filterState.studentsLoaded && !selectorIsEmpty) {
            return Promise.resolve();
        }

        // Marcar que la carga está en progreso
        filterState.loadingInProgress = true;

        // Actualizar estado
        filterState.lastCourseId = courseId;
        filterState.lastForce = normalizedForce;
        filterState.lastWingType = normalizedWingType;
        filterState.lastPhaseId = normalizedPhaseId;

        // Mostrar loading
        showLoadingState(selectors.student, 'Cargando estudiantes...');

        // Determinar si debe habilitar el selector de estudiantes
        const shouldEnableStudents = !!courseId;

        // Llamar a la API
        return fetchStudents(courseId, normalizedForce, normalizedWingType, normalizedPhaseId)
            .then(students => {
                populateSelect(selectors.student, students, 'Seleccione un estudiante...');

                if (shouldEnableStudents) {
                    enableSelect(selectors.student, 'Disponible al seleccionar un programa');
                    filterState.studentsLoaded = true;
                } else {
                    disableSelect(selectors.student, 'Primero complete los filtros necesarios');
                    filterState.studentsLoaded = false;
                }

                // Marcar que la carga terminó
                filterState.loadingInProgress = false;

                updateButtonsVisibility();
            })
            .catch(error => {
                console.error('Error en fetchStudents:', error);
                handleError('Error al cargar los estudiantes', error);
                disableSelect(selectors.student, 'Error al cargar estudiantes');
                filterState.studentsLoaded = false;

                // Marcar que la carga terminó (incluso si hubo error)
                filterState.loadingInProgress = false;

                updateButtonsVisibility();
            });
    }

    /**
     * Maneja el cambio en el selector de estudiante
     */
    function handleStudentChange() {
        updateIndividualReportInputs();
        updateButtonsVisibility();
    }

    /**
     * Realiza una petición a la API para obtener fuerzas
     * @param {string} courseId - ID del programa
     * @returns {Promise<Array>} - Promesa con el array de fuerzas
     */
    function fetchForces(courseId) {
        return $.ajax({
            url: '/api/reports/forces',
            method: 'GET',
            data: { courseId: courseId },
            dataType: 'json'
        }).then(response => {
            if (response.success && response.data) {
                return response.data;
            }
            throw new Error(response.message || 'Error al obtener las fuerzas');
        });
    }

    /**
     * Realiza una petición a la API para obtener tipos de ala
     * @param {string} courseId - ID del programa
     * @param {string} force - Fuerza seleccionada
     * @returns {Promise<Array>} - Promesa con el array de tipos de ala
     */
    function fetchWingTypes(courseId, force) {
        return $.ajax({
            url: '/api/reports/wing-types',
            method: 'GET',
            data: {
                courseId: courseId,
                force: force
            },
            dataType: 'json'
        }).then(response => {
            if (response.success && response.data) {
                return response.data;
            }
            throw new Error(response.message || 'Error al obtener los tipos de ala');
        });
    }

    /**
     * Realiza una petición a la API para obtener fases
     * @param {string} wingType - Tipo de ala seleccionado
     * @returns {Promise<Array>} - Promesa con el array de fases
     */
    function fetchPhases(wingType) {
        return $.ajax({
            url: '/api/reports/phases',
            method: 'GET',
            data: { wingType: wingType },
            dataType: 'json'
        }).then(response => {
            if (response.success && response.data) {
                return response.data;
            }
            throw new Error(response.message || 'Error al obtener las fases');
        });
    }

    /**
     * Realiza una petición a la API para obtener estudiantes
     * @param {string} courseId - ID del programa
     * @param {string} force - Fuerza seleccionada
     * @param {string} wingType - Tipo de ala seleccionado
     * @param {string} phaseId - ID de la fase (opcional)
     * @returns {Promise<Array>} - Promesa con el array de estudiantes
     */
    function fetchStudents(courseId, force, wingType, phaseId = null) {
        const data = {
            courseId: courseId,
            force: force,
            wingType: wingType
        };

        if (phaseId) {
            data.phaseId = phaseId;
        }

        return $.ajax({
            url: '/api/reports/students',
            method: 'GET',
            data: data,
            dataType: 'json'
        }).then(response => {
            if (response.success && response.data) {
                return response.data;
            }
            throw new Error(response.message || 'Error al obtener los estudiantes');
        });
    }

    /**
     * Puebla un selector con opciones
     * @param {jQuery} selector - Elemento select
     * @param {Array} items - Array de items a agregar
     * @param {string} placeholder - Texto del placeholder
     */
    function populateSelect(selector, items, placeholder) {
        // Limpiar opciones existentes excepto la primera
        selector.find('option:not(:first)').remove();

        // Actualizar placeholder
        selector.find('option:first').text(placeholder);

        // Agregar nuevas opciones
        if (items && items.length > 0) {
            items.forEach(item => {
                const option = new Option(item.text || item.name, item.value || item.id, false, false);
                selector.append(option);
            });
        }

        // Actualizar Select2
        selector.trigger('change.select2');
    }

    /**
     * Habilita un selector
     * @param {jQuery} selector - Elemento select
     * @param {string} helpText - Texto de ayuda
     */
    function enableSelect(selector, helpText) {
        selector.prop('disabled', false);
        selector.closest('.mb-4').find('.form-text').text(helpText);
    }

    /**
     * Deshabilita un selector
     * @param {jQuery} selector - Elemento select
     * @param {string} helpText - Texto de ayuda
     */
    function disableSelect(selector, helpText) {
        selector.prop('disabled', true);
        selector.val('').trigger('change');
        selector.closest('.mb-4').find('.form-text').text(helpText);
    }

    /**
     * Resetea un selector a su estado inicial
     * @param {jQuery} selector - Elemento select
     */
    function resetSelect(selector) {
        selector.find('option:not(:first)').remove();
        selector.val('').trigger('change');
    }

    /**
     * Muestra estado de carga en un selector
     * @param {jQuery} selector - Elemento select
     * @param {string} message - Mensaje de carga
     */
    function showLoadingState(selector, message) {
        selector.closest('.mb-4').find('.form-text').html(
            `<span class="text-info"><i class="ti ti-loader"></i> ${message}</span>`
        );
    }

    /**
     * Resetea desde el selector de fuerza hacia adelante
     */
    function resetFromForce() {
        disableSelect(selectors.force, 'Seleccione un programa primero');
        resetFromWingType();
    }

    /**
     * Resetea desde el selector de tipo de ala hacia adelante
     */
    function resetFromWingType() {
        disableSelect(selectors.wingType, 'Seleccione una fuerza primero');
        resetFromPhase();
    }

    /**
     * Resetea desde el selector de fase hacia adelante
     */
    function resetFromPhase() {
        disableSelect(selectors.phase, 'Seleccione tipo de ala primero');
        resetFromStudent();
    }

    /**
     * Resetea el selector de estudiante
     */
    function resetFromStudent() {
        resetSelect(selectors.student);
        // Si hay un programa seleccionado, mantener habilitado el selector
        // sino deshabilitarlo
        if (selectors.course.val()) {
            // Habilitar pero mostrar mensaje de carga
            selectors.student.prop('disabled', false);
            selectors.student.closest('.mb-4').find('.form-text').text('Cargando estudiantes...');
        } else {
            disableSelect(selectors.student, 'Primero seleccione un programa');
        }
    }

    /**
     * Actualiza los inputs hidden del formulario de informe general
     */
    function updateGeneralReportInputs() {
        hiddenInputs.generalCourseId.val(selectors.course.val());
        // Normalizar valores vacíos a 'TODAS' para filtros opcionales
        hiddenInputs.generalForce.val(selectors.force.val() || 'TODAS');
        hiddenInputs.generalWingType.val(selectors.wingType.val() || 'TODAS');
        hiddenInputs.generalPhaseId.val(selectors.phase.val() || '');
    }

    /**
     * Actualiza los inputs hidden del formulario de informe individual
     */
    function updateIndividualReportInputs() {
        hiddenInputs.individualCourseId.val(selectors.course.val());
        // Normalizar valores vacíos a 'TODAS' para que el backend los procese correctamente
        hiddenInputs.individualForce.val(selectors.force.val() || 'TODAS');
        hiddenInputs.individualWingType.val(selectors.wingType.val() || 'TODAS');
        hiddenInputs.individualPhaseId.val(selectors.phase.val() || '');
        hiddenInputs.individualStudentId.val(selectors.student.val());
    }

    /**
     * Actualiza la visibilidad de los botones según las selecciones
     */
    function updateButtonsVisibility() {
        const courseSelected = !!selectors.course.val();
        const studentSelected = !!selectors.student.val();

        // Mostrar botón de informe general solo con programa seleccionado
        // Los selectores 2, 3 y 4 son opcionales (se envía "TODAS" por defecto)
        const showGeneralReport = courseSelected;

        // Permitir informe individual si hay programa y estudiante seleccionado
        const showIndividualReport = courseSelected && studentSelected;

        // Mostrar/ocultar formularios
        if (showGeneralReport) {
            // Actualizar inputs del informe general
            updateGeneralReportInputs();
            forms.general.fadeIn();
            noSelectionMessage.fadeOut();
        } else {
            forms.general.fadeOut();
            if (!showIndividualReport) {
                noSelectionMessage.fadeIn();
            }
        }

        if (showIndividualReport) {
            // Actualizar inputs del informe individual
            updateIndividualReportInputs();
            forms.individual.fadeIn();
            if (!showGeneralReport) {
                noSelectionMessage.fadeOut();
            }
        } else {
            forms.individual.fadeOut();
        }
    }

    /**
     * Maneja errores de las peticiones AJAX
     * @param {string} message - Mensaje de error
     * @param {Error} error - Objeto de error
     */
    function handleError(message, error) {
        console.error(message, error);

        // Mostrar notificación con SweetAlert2 si está disponible
        if (typeof Swal !== 'undefined') {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: message,
                confirmButtonText: 'Aceptar'
            });
        } else {
            alert(message);
        }
    }

    /**
     * Inicialización cuando el DOM está listo
     */
    $(document).ready(function () {
        initializeEventListeners();
        updateButtonsVisibility();
    });

})(jQuery);
