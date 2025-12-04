// ===========================================
// MÓDULO DE EDICIÓN DE CALIFICACIONES
// ===========================================

import { openNRedModal, cancelAndRevertGrade, clearTaskReasons } from './edit-nred-modal.js';

// Variables globales
let currentFailedMissionsInWindow = 0;
let currentTotalFailedMissions = 0;
let isMissionCurrentlyFailed = false;
let originalGrades = new Map(); // Para trackear cambios

/**
 * Inicializa el formulario de edición de calificaciones
 */
export function initializeEditGradesForm(failedInWindow, totalFailed, missionFailed) {
    currentFailedMissionsInWindow = failedInWindow;
    currentTotalFailedMissions = totalFailed;
    isMissionCurrentlyFailed = missionFailed;

    // Guardar calificaciones originales
    saveOriginalGrades();

    // Configurar listeners
    setupGradeChangeListeners();
    setupFormSubmitValidation();
}

/**
 * Guarda las calificaciones originales para poder detectar cambios
 */
function saveOriginalGrades() {
    document.querySelectorAll('.edit-task-grade:checked').forEach(radio => {
        const taskIndex = radio.dataset.taskIndex;
        originalGrades.set(taskIndex, radio.value);
    });
}

/**
 * Configura los listeners para cambios de calificación
 */
function setupGradeChangeListeners() {
    document.querySelectorAll('.edit-task-grade').forEach(radio => {
        radio.addEventListener('change', handleGradeChange);
    });
}

/**
 * Maneja el cambio de calificación
 */
async function handleGradeChange(event) {
    const radio = event.target;
    const taskIndex = radio.dataset.taskIndex;
    const taskId = radio.dataset.taskId;
    const taskName = radio.dataset.taskName;
    const isP3 = radio.dataset.isP3 === 'True' || radio.dataset.isP3 === 'true';
    const newGrade = radio.value;
    const oldGrade = originalGrades.get(taskIndex);

    // Si selecciona N o NR, abrir modal para razones
    if (newGrade === 'N' || newGrade === 'NR') {
        // Abrir modal de razones
        openNRedModal(taskIndex, taskId, taskName, isP3, newGrade);

        // Calcular impacto y mostrar alertas si es necesario
        await calculateAndShowImpactAlerts(taskIndex, newGrade, isP3, oldGrade);
    } else {
        // Si cambia de N/NR a otra nota, limpiar razones
        clearNRedReasons(taskIndex);
    }
}

/**
 * Calcula el impacto del cambio y muestra alertas si es necesario
 */
async function calculateAndShowImpactAlerts(taskIndex, newGrade, isP3, oldGrade) {
    // Determinar si la nueva calificación es N-Roja
    const isNewGradeNRed = (newGrade === 'NR' && isP3);

    // Si la tarea cambia a N-Roja
    if (isNewGradeNRed) {
        const impact = calculateMissionImpact();

        // Solo mostrar alerta si la misión cambia de aprobada a fallida
        if (impact.changesStatus && !isMissionCurrentlyFailed) {
            const failedInWindow = impact.failedInWindow;
            const totalFailed = impact.totalFailed;

            // REGLA 1: Verificar si alcanza 3 misiones fallidas en ventana (comité)
            if (failedInWindow >= 3) {
                const result = await Swal.fire({
                    title: 'Advertencia: Comité',
                    html: `
                        <div class="text-start">
                            <p><strong>Esta acción enviará al estudiante a COMITÉ</strong></p>
                            <p class="mb-2">Razón: Alcanzará <strong>${failedInWindow} misiones fallidas</strong> en la ventana de las últimas 5 misiones.</p>
                            <p class="text-danger mb-0"><i class="ti ti-alert-triangle me-1"></i>¿Desea continuar con este cambio?</p>
                        </div>
                    `,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Sí, continuar',
                    cancelButtonText: 'No, cancelar',
                    customClass: {
                        popup: 'swal2-committee-alert'
                    }
                });

                if (!result.isConfirmed) {
                    // Revertir la selección de calificación
                    cancelAndRevertGrade();
                    return;
                }
            }

            // REGLA 2: Verificar si alcanza 15 misiones fallidas totales (suspensión)
            if (totalFailed >= 15) {
                const result = await Swal.fire({
                    title: 'Advertencia: Suspensión',
                    html: `
                        <div class="text-start">
                            <p><strong>Esta acción SUSPENDERÁ al estudiante del programa</strong></p>
                            <p class="mb-2">Razón: Alcanzará <strong>${totalFailed} misiones fallidas</strong> en total (máximo: 15).</p>
                            <p class="text-danger mb-0"><i class="ti ti-alert-triangle me-1"></i>Esta acción es IRREVERSIBLE. ¿Está seguro de continuar?</p>
                        </div>
                    `,
                    icon: 'error',
                    showCancelButton: true,
                    confirmButtonText: 'Sí, estoy seguro',
                    cancelButtonText: 'No, cancelar',
                    customClass: {
                        popup: 'swal2-suspension-alert'
                    }
                });

                if (!result.isConfirmed) {
                    // Revertir la selección de calificación
                    cancelAndRevertGrade();
                    return;
                }
            }
        }
    }
}

/**
 * Calcula el impacto del cambio en misiones fallidas
 */
function calculateMissionImpact() {
    // Contar N-Rojas después de las ediciones actuales
    let nRedCount = 0;

    document.querySelectorAll('.edit-task-grade:checked').forEach(radio => {
        const grade = radio.value;

        if (grade === 'NR') {
            nRedCount++;
        }
    });

    const wouldFailMission = nRedCount > 0;

    // Si la misión cambia de aprobada a fallida
    if (wouldFailMission && !isMissionCurrentlyFailed) {
        return {
            changesStatus: true,
            failedInWindow: currentFailedMissionsInWindow + 1,
            totalFailed: currentTotalFailedMissions + 1,
            nRedCount: nRedCount
        };
    }

    // Si la misión cambia de fallida a aprobada
    if (!wouldFailMission && isMissionCurrentlyFailed) {
        return {
            changesStatus: true,
            failedInWindow: Math.max(0, currentFailedMissionsInWindow - 1),
            totalFailed: Math.max(0, currentTotalFailedMissions - 1),
            nRedCount: nRedCount
        };
    }

    return {
        changesStatus: false,
        failedInWindow: currentFailedMissionsInWindow,
        totalFailed: currentTotalFailedMissions,
        nRedCount: nRedCount
    };
}

/**
 * Obtiene el contenedor específico de razones N-Red para una tarea
 */
function getNRedReasonsContainer(taskIndex) {
    return document.querySelector(`.nred-reasons-container[data-task-index="${taskIndex}"]`);
}

/**
 * Limpia las razones de N-Roja de una tarea
 */
function clearNRedReasons(taskIndex) {
    // Usar la función del módulo modal que limpia tanto DOM como memoria
    clearTaskReasons(taskIndex);
}

/**
 * Configura la validación del formulario antes de enviar
 */
function setupFormSubmitValidation() {
    const form = document.getElementById('editGradesForm');
    if (!form) return;

    form.addEventListener('submit', async (event) => {
        event.preventDefault();

        // Validar que todas las tareas con N/NR tengan razones
        const isValid = validateNRedReasons();

        if (!isValid) {
            await Swal.fire({
                title: 'Validación Requerida',
                text: 'Todas las tareas con calificación N o N-Roja deben tener razones especificadas.',
                icon: 'warning',
                confirmButtonText: 'Entendido'
            });
            return;
        }

        // Verificar si hay cambios
        const hasChanges = checkForChanges();

        if (!hasChanges) {
            await Swal.fire({
                title: 'Sin Cambios',
                text: 'No se detectaron cambios en las calificaciones.',
                icon: 'info',
                confirmButtonText: 'Entendido'
            });
            return;
        }

        // Mostrar confirmación final
        const result = await Swal.fire({
            title: '¿Guardar Cambios?',
            text: 'Se guardarán las modificaciones realizadas a las calificaciones.',
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#0d6efd',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Sí, guardar',
            cancelButtonText: 'Cancelar'
        });

        if (result.isConfirmed) {
            // Enviar formulario
            form.submit();
        }
    });
}

/**
 * Valida que todas las tareas con N/NR tengan razones
 */
function validateNRedReasons() {
    let isValid = true;

    document.querySelectorAll('.edit-task-grade:checked').forEach(radio => {
        const grade = radio.value;
        const taskIndex = radio.dataset.taskIndex;

        // Si es N o NR, verificar que tenga razones
        if (grade === 'N' || grade === 'NR') {
            const container = getNRedReasonsContainer(taskIndex);
            if (!container) {
                isValid = false;
                return;
            }

            const reasonFields = container.querySelectorAll(
                `input[name^="TaskGrades[${taskIndex}].NRedReasons"]`
            );

            if (reasonFields.length === 0) {
                isValid = false;

                // Resaltar el contenedor de la tarea completa
                const taskCard = document.querySelector(`.task-edit-card[data-task-index="${taskIndex}"]`);
                if (taskCard) {
                    taskCard.classList.add('border', 'border-danger');
                    setTimeout(() => {
                        taskCard.classList.remove('border', 'border-danger');
                    }, 3000);
                }
            }
        }
    });

    return isValid;
}

/**
 * Verifica si hubo cambios en las calificaciones
 */
function checkForChanges() {
    let hasChanges = false;

    document.querySelectorAll('.edit-task-grade:checked').forEach(radio => {
        const taskIndex = radio.dataset.taskIndex;
        const currentGrade = radio.value;
        const originalGrade = originalGrades.get(taskIndex);

        if (currentGrade !== originalGrade) {
            hasChanges = true;
        }
    });

    return hasChanges;
}
