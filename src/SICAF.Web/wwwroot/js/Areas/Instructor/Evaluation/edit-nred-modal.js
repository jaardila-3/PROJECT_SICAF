// ===========================================
// MÓDULO DE GESTIÓN DE MODAL N-RED PARA EDICIÓN
// Específico para EditMissionGrades.cshtml
// ===========================================

import { NRED_REASONS } from './nred-reasons-data.js';

// Variables globales del módulo
let modalInstance = null;
let currentTaskData = null;
let selectedReasons = new Map();

/**
 * Inicializa el módulo del modal N-Red para edición
 */
export function initializeEditNRedModal() {
    const modalElement = document.getElementById('modalNRedReasons');
    if (!modalElement) return;

    modalInstance = new bootstrap.Modal(modalElement);

    setupAccordionListeners(modalElement);
    setupButtonListeners(modalElement);
    setupGradeButtonListeners();
}

/**
 * Configura los listeners para los botones de calificación N y N-Roja
 * Permite reabrir el modal cuando se hace clic en una nota ya seleccionada
 */
function setupGradeButtonListeners() {
    // Buscar todos los radio buttons de N y N-Roja en el formulario de edición
    const gradeInputs = document.querySelectorAll('input.edit-task-grade[value="N"], input.edit-task-grade[value="NR"]');

    gradeInputs.forEach(input => {
        input.addEventListener('click', function () {
            // Si el radio button ya estaba seleccionado y se hace clic de nuevo
            // abrir el modal para ver/editar razones
            const taskIndex = this.dataset.taskIndex;
            const taskId = this.dataset.taskId;
            const taskName = this.dataset.taskName;
            const isP3 = this.dataset.isP3;
            const grade = this.value;

            // Solo abrir si ya está checked (segundo clic)
            if (this.checked) {
                // Abrir el modal (ya sea para ver razones existentes o para agregarlas)
                setTimeout(() => {
                    openNRedModal(taskIndex, taskId, taskName, isP3, grade);
                }, 50);
            }
        });
    });
}

/**
 * Abre el modal para una tarea específica
 */
export function openNRedModal(taskIndex, taskId, taskName, isP3, grade) {
    currentTaskData = {
        taskIndex: parseInt(taskIndex),
        taskId: taskId,
        taskName: taskName,
        isP3: isP3 === 'true' || isP3 === 'True',
        grade: grade
    };

    // Configurar información de la tarea en el modal
    document.getElementById('modalTaskName').textContent = taskName;
    document.getElementById('modalTaskIndex').value = taskIndex;
    document.getElementById('modalTaskId').value = taskId;

    // Primero resetear, luego cargar selecciones previas
    resetModalState();

    // Verificar si hay selecciones guardadas en el DOM (campos hidden)
    loadSelectionsFromDOM(taskIndex);

    modalInstance.show();
}

/**
 * Obtiene el contenedor específico de razones N-Red para una tarea
 * Solo para EditMissionGrades (usa .nred-reasons-container)
 */
function getNRedReasonsContainer(taskIndex) {
    return document.querySelector(`.nred-reasons-container[data-task-index="${taskIndex}"]`);
}

/**
 * Carga las selecciones desde los campos hidden del DOM
 */
function loadSelectionsFromDOM(taskIndex) {
    const container = getNRedReasonsContainer(taskIndex);
    if (!container) {
        console.warn(`No se encontró contenedor para tarea ${taskIndex}`);
        return;
    }

    // Buscar todos los campos hidden de esta tarea
    const categoryFields = container.querySelectorAll(`input[name*="TaskGrades[${taskIndex}].NRedReasons"][name*="ReasonCategory"]`);
    const descriptionFields = container.querySelectorAll(`input[name*="TaskGrades[${taskIndex}].NRedReasons"][name*="ReasonDescription"]`);

    if (categoryFields.length === 0) {
        // Si no hay campos hidden pero sí hay selecciones en memoria, cargarlas
        loadSelectionsFromMemory(taskIndex);
        return;
    }

    // Agrupar las selecciones por categoría
    const selectionsByCategory = {};

    categoryFields.forEach((field, index) => {
        const category = field.value;
        const description = descriptionFields[index]?.value;

        if (category && description) {
            if (!selectionsByCategory[category]) {
                selectionsByCategory[category] = [];
            }
            selectionsByCategory[category].push(description);
        }
    });

    // Expandir acordeones y marcar checkboxes
    Object.keys(selectionsByCategory).forEach(category => {
        const reasonsContainer = document.querySelector(`.reasons-container[data-category="${category}"]`);
        if (reasonsContainer) {
            // Cargar las razones
            loadReasonsForCategory(category, reasonsContainer);

            // Expandir el acordeón
            const collapseElement = reasonsContainer.closest('.accordion-collapse');
            if (collapseElement) {
                bootstrap.Collapse.getOrCreateInstance(collapseElement).show();
            }

            // Marcar las razones seleccionadas
            setTimeout(() => {
                selectionsByCategory[category].forEach(description => {
                    const checkbox = reasonsContainer.querySelector(`.reason-checkbox[value="${description}"]`);
                    if (checkbox) {
                        checkbox.checked = true;
                    }
                });
            }, 150);
        }
    });
}

/**
 * Carga selecciones desde la memoria (Map)
 */
function loadSelectionsFromMemory(taskIndex) {
    const taskIndexInt = parseInt(taskIndex);

    if (!selectedReasons.has(taskIndexInt)) {
        console.log(`No hay selecciones en memoria para tarea ${taskIndex}`);
        return;
    }

    const previousSelections = selectedReasons.get(taskIndexInt);
    console.log(`Cargando ${previousSelections.size} razones desde memoria para tarea ${taskIndex}`);

    // Agrupar por categoría
    const selectionsByCategory = {};
    previousSelections.forEach(selection => {
        if (!selectionsByCategory[selection.category]) {
            selectionsByCategory[selection.category] = [];
        }
        selectionsByCategory[selection.category].push(selection.description);
    });

    // Para cada categoría con selecciones previas
    Object.keys(selectionsByCategory).forEach(category => {
        const reasonsContainer = document.querySelector(`.reasons-container[data-category="${category}"]`);
        if (reasonsContainer) {
            // Cargar las razones
            loadReasonsForCategory(category, reasonsContainer);

            // Expandir el acordeón
            const collapseElement = reasonsContainer.closest('.accordion-collapse');
            if (collapseElement) {
                bootstrap.Collapse.getOrCreateInstance(collapseElement).show();
            }

            // Marcar las razones seleccionadas
            setTimeout(() => {
                selectionsByCategory[category].forEach(description => {
                    const checkbox = reasonsContainer.querySelector(`.reason-checkbox[value="${description}"]`);
                    if (checkbox) {
                        checkbox.checked = true;
                    }
                });
            }, 150);
        }
    });
}

/**
 * Limpia las selecciones de una tarea
 */
export function clearTaskReasons(taskIndex) {
    selectedReasons.delete(parseInt(taskIndex));

    const container = getNRedReasonsContainer(taskIndex);
    if (container) {
        container.innerHTML = '';
        container.style.display = 'none';
    }
}

/**
 * Configura los listeners de los acordeones
 */
function setupAccordionListeners(modalElement) {
    modalElement.querySelectorAll('.accordion-collapse').forEach(collapseElement => {
        collapseElement.addEventListener('show.bs.collapse', function () {
            const reasonsContainer = this.querySelector('.reasons-container');
            if (!reasonsContainer) {
                console.warn("Se abrió un acordeón sin 'reasons-container'.", this);
                return;
            }
            const category = reasonsContainer.dataset.category;

            // Cargar razones solo si tiene el spinner
            if (reasonsContainer.querySelector('.spinner-border')) {
                loadReasonsForCategory(category, reasonsContainer);
            }
        });
    });
}

/**
 * Configura los botones del modal
 */
function setupButtonListeners(modalElement) {
    modalElement.querySelector('#btnSaveNRed').addEventListener('click', function () {
        if (validateSelections()) {
            saveSelectionsAndClose();
        }
    });

    modalElement.querySelector('#btnCancelNRed').addEventListener('click', function () {
        cancelAndRevertGrade();
    });
}

/**
 * Carga las razones de una categoría
 */
function loadReasonsForCategory(category, container) {
    const reasons = NRED_REASONS[category] || [];

    container.innerHTML = '';

    if (reasons.length > 0) {
        const reasonsHtml = reasons.map(reason => {
            const reasonId = `reason_${category.replace(/\s/g, '_')}_${reason.replace(/\s/g, '_')}`;

            return `
                <div class="form-check mb-2">
                    <input class="form-check-input reason-checkbox"
                           type="checkbox"
                           value="${reason}"
                           id="${reasonId}"
                           data-category="${category}">
                    <label class="form-check-label" for="${reasonId}">
                        ${reason}
                    </label>
                </div>
            `;
        }).join('');

        container.innerHTML = reasonsHtml;
    } else {
        container.innerHTML = '<p class="text-muted">No hay razones disponibles para esta categoría</p>';
    }
}

/**
 * Resetea el estado del modal
 */
function resetModalState() {
    // Limpiar todos los checkboxes
    document.querySelectorAll('.reason-checkbox').forEach(cb => {
        cb.checked = false;
    });

    // Colapsar todos los acordeones
    document.querySelectorAll('.accordion-collapse').forEach(collapse => {
        const bsCollapse = bootstrap.Collapse.getInstance(collapse);
        if (bsCollapse) {
            bsCollapse.hide();
        }
    });

    // Restablecer los spinners en los contenedores
    document.querySelectorAll('.reasons-container').forEach(container => {
        container.innerHTML = `
            <div class="d-flex justify-content-center">
                <div class="spinner-border spinner-border-sm text-primary" role="status">
                    <span class="visually-hidden">Cargando...</span>
                </div>
            </div>
        `;
    });

    // Ocultar mensaje de validación
    const validationMessage = document.getElementById('modalValidationMessage');
    if (validationMessage) {
        validationMessage.classList.add('d-none');
    }
}

/**
 * Valida las selecciones
 */
function validateSelections() {
    const validationMessage = document.getElementById('modalValidationMessage');
    const validationText = document.getElementById('modalValidationText');

    const selectedReasonCheckboxes = document.querySelectorAll('.reason-checkbox:checked');

    if (selectedReasonCheckboxes.length === 0) {
        validationText.textContent = 'Debe seleccionar al menos una razón';
        validationMessage.classList.remove('d-none');
        return false;
    }

    validationMessage.classList.add('d-none');
    return true;
}

/**
 * Guarda las selecciones y cierra el modal
 */
function saveSelectionsAndClose() {
    const taskIndex = currentTaskData.taskIndex;
    const container = getNRedReasonsContainer(taskIndex);

    if (!container) {
        console.error(`No se encontró contenedor de razones N-Red para tarea ${taskIndex}`);
        return;
    }

    // Limpiar contenedor
    container.innerHTML = '';
    container.style.display = 'block';

    const selections = new Set();
    let fieldIndex = 0;

    // Obtener todas las razones seleccionadas
    document.querySelectorAll('.reason-checkbox:checked').forEach(checkbox => {
        const category = checkbox.dataset.category;
        const description = checkbox.value;

        // Guardar en el Set
        selections.add({ category, description });

        // Crear campos hidden
        const categoryField = document.createElement('input');
        categoryField.type = 'hidden';
        categoryField.name = `TaskGrades[${taskIndex}].NRedReasons[${fieldIndex}].ReasonCategory`;
        categoryField.value = category;
        container.appendChild(categoryField);

        const descriptionField = document.createElement('input');
        descriptionField.type = 'hidden';
        descriptionField.name = `TaskGrades[${taskIndex}].NRedReasons[${fieldIndex}].ReasonDescription`;
        descriptionField.value = description;
        container.appendChild(descriptionField);
        fieldIndex++;
    });

    // Guardar en memoria también
    selectedReasons.set(taskIndex, selections);

    modalInstance.hide();
}

/**
 * Cancela y revierte la calificación a la original
 * Específico para modo edición
 */
export function cancelAndRevertGrade() {
    const taskIndex = currentTaskData.taskIndex;

    // Buscar el campo OldGrade para obtener la calificación original
    const oldGradeInput = document.querySelector(`input[name="TaskGrades[${taskIndex}].OldGrade"]`);
    const originalGrade = oldGradeInput ? oldGradeInput.value : null;

    // Buscar todos los radios de esta tarea
    const gradeInputs = document.querySelectorAll(`input[name="TaskGrades[${taskIndex}].NewGrade"]`);

    // Revertir a la calificación original
    gradeInputs.forEach(input => {
        input.checked = (input.value === originalGrade);
    });

    // Limpiar las razones
    clearTaskReasons(taskIndex);

    modalInstance.hide();
}
