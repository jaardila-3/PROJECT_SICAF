/**
 * SICAF - Evaluación de Estudiantes
 * Gestión de evaluaciones, calificaciones y decisiones de comité
 */

import { initializeNRedModal, openNRedModal, clearTaskReasons } from './nred-modal.js';

// ==========================================
// INICIALIZACIÓN
// ==========================================

document.addEventListener('DOMContentLoaded', function () {
    initializeGradeHandlers();
    initializeFormValidation();
    initializeCustomValidations();
    initializeCommitteeModal();
    initializePromotePhaseButton();
    initializeFinalizeApproveButton();
    initializeNRedModal();
    initializeMissionEvaluableSwitch();
});

// ==========================================
// MANEJADORES DE CALIFICACIONES
// ==========================================

/**
 * Inicializa los manejadores de eventos para los botones de calificación
 */
function initializeGradeHandlers() {
    // Manejadores para tareas regulares
    const regularGradeRadios = document.querySelectorAll('input[type="radio"][name*="TaskGrades"]');
    regularGradeRadios.forEach(radio => {
        radio.addEventListener('click', function () {
            handleRegularTaskGradeChange(this);
        });
    });
}

/**
 * Maneja el cambio de calificación para tareas regulares
 * @param {HTMLInputElement} radio - El radio button seleccionado
 */
function handleRegularTaskGradeChange(radio) {
    const grade = radio.value;
    const taskContainer = radio.closest('.task-evaluation-card');
    const container = taskContainer.querySelector('.nred-fields-container');

    if (!container) return;

    const taskIndex = container.dataset.taskIndex;
    const taskId = container.dataset.taskId;
    const taskName = container.dataset.taskName;
    const isP3 = container.dataset.isP3;

    // Si es N o NR, abrir el modal
    if ((grade === 'N' || grade === 'NR') && radio.checked) {
        openNRedModal(taskIndex, taskId, taskName, isP3, grade);
    } else {
        // Si cambió a otra calificación, limpiar razones
        clearTaskReasons(taskIndex);
    }
}

// ==========================================
// VALIDACIÓN DE FORMULARIO
// ==========================================

/**
 * Inicializa la validación y submit del formulario de evaluación
 */
function initializeFormValidation() {
    const form = document.getElementById('evaluationForm');
    if (!form) {
        console.log('No esta evaluando a un estudiante, estudiante con novedad.');
        return;
    }

    form.addEventListener('submit', async function (e) {

        // Prevenir el envío por defecto
        e.preventDefault();
        e.stopPropagation();

        // Validar que todas las tareas OBLIGATORIAS estén calificadas
        if (!validateTaskGrades()) {
            console.log('Validación de tareas falló');
            form.classList.add('was-validated');
            return false;
        }

        // Validar campos de Bootstrap
        if (!form.checkValidity()) {
            console.log('Formulario no pasó validación de Bootstrap');
            form.classList.add('was-validated');

            // Mostrar qué campos están inválidos
            const invalidFields = form.querySelectorAll(':invalid');
            console.log('Campos inválidos:', invalidFields);

            if (typeof Swal !== 'undefined') {
                Swal.fire({
                    icon: 'warning',
                    title: 'Campos incompletos',
                    text: 'Por favor complete todos los campos requeridos del formulario.',
                    confirmButtonText: 'Entendido'
                });
            }
            return false;
        }

        // Si todo está bien, enviar via AJAX
        form.classList.add('was-validated');
        await submitEvaluationForm(form);
    });
}

/**
 * Valida que todas las tareas tengan calificación
 * @returns {boolean} - True si todas las tareas están calificadas
 */
function validateTaskGrades() {
    const radioGroups = {};
    const radios = document.querySelectorAll('input[type="radio"].regular-task-grade');

    radios.forEach(radio => {
        if (!radioGroups[radio.name]) {
            radioGroups[radio.name] = false;
        }
        if (radio.checked) {
            radioGroups[radio.name] = true;
        }
    });

    const ungraded = Object.keys(radioGroups).filter(group => !radioGroups[group]);

    if (ungraded.length > 0) {
        if (typeof Swal !== 'undefined') {
            Swal.fire('Advertencia', `Por favor califique todas las tareas. Faltan ${ungraded.length} tareas por calificar.`, 'error');
        } else {
            alert(`Por favor califique todas las tareas. Faltan ${ungraded.length} tareas por calificar.`);
        }
        return false;
    }

    // Validar campos obligatorios de N-Roja
    return validateNRedFields();
}

/**
 * Valida los campos de N y N-Roja 
 * @returns {boolean} - True si todos los campos son válidos
 */
function validateNRedFields() {
    let isValid = true;

    // Buscar todas las tareas con calificación N o NR
    document.querySelectorAll('.regular-task-grade:checked').forEach(radio => {
        const grade = radio.value;
        if (grade === 'N' || grade === 'NR') {
            const taskCard = radio.closest('.task-evaluation-card');
            const container = taskCard.querySelector('.nred-fields-container');

            // Verificar que tenga al menos un campo hidden (una razón)
            const hasReasons = container && container.querySelector('input[type="hidden"]');

            if (!hasReasons) {
                isValid = false;

                Swal.fire({
                    icon: 'warning',
                    title: 'Razones para N o N-Roja requeridas',
                    text: `La tarea "${container?.dataset.taskName || 'Sin nombre'}" tiene calificación ${grade} pero no tiene razones seleccionadas.`,
                    confirmButtonText: 'Seleccionar Razones'
                }).then((result) => {
                    if (result.isConfirmed) {
                        // Abrir el modal para esa tarea
                        openNRedModal(
                            container.dataset.taskIndex,
                            container.dataset.taskId,
                            container.dataset.taskName,
                            container.dataset.isP3,
                            grade
                        );
                    }
                });

                return false; // Salir del forEach
            }
        }
    });

    return isValid;
}

/**
 * Envía el formulario de evaluación via AJAX
 * @param {HTMLFormElement} form - El formulario a enviar
 */
async function submitEvaluationForm(form) {
    // Recopilar datos del formulario
    const formData = collectFormData(form);

    // Mostrar loading
    Swal.fire({
        title: 'Guardando evaluación...',
        html: 'Por favor espere mientras se procesa la información.',
        allowOutsideClick: false,
        allowEscapeKey: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });

    try {
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
        const response = await fetch('/Instructor/Evaluation/SaveEvaluation', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify(formData)
        });

        const result = await response.json();

        if (result.success) {
            await Swal.fire({
                icon: 'success',
                title: 'Evaluación Guardada',
                text: result.message || 'La evaluación se ha guardado exitosamente',
                confirmButtonText: 'Aceptar'
            });

            // Redirigir al detalle del programa
            if (result.data?.redirectUrl) {
                window.location.href = result.data.redirectUrl;
            }
        } else {
            await Swal.fire({
                icon: 'error',
                title: 'Error al Guardar',
                text: result.message || 'No se pudo guardar la evaluación',
                confirmButtonText: 'Entendido'
            });
        }
    } catch (error) {
        console.error('Error al enviar formulario:', error);
        await Swal.fire({
            icon: 'error',
            title: 'Error de Conexión',
            text: 'Ocurrió un error al procesar la solicitud. Por favor intente nuevamente.',
            confirmButtonText: 'Aceptar'
        });
    }
}

/**
 * Recopila todos los datos del formulario en un objeto JSON
 * @param {HTMLFormElement} form - El formulario del cual recopilar datos
 * @returns {Object} - Objeto con todos los datos del formulario
 */
function collectFormData(form) {
    const formData = {
        StudentId: form.querySelector('input[name="StudentId"]').value,
        InstructorId: form.querySelector('input[name="InstructorId"]').value,
        MissionId: form.querySelector('input[name="MissionId"]').value,
        PhaseId: form.querySelector('input[name="PhaseId"]').value,
        CourseId: form.querySelector('input[name="CourseId"]').value,
        AircraftId: form.querySelector('select[name="AircraftId"]').value,
        GeneralObservations: form.querySelector('textarea[name="GeneralObservations"]').value,
        MachineFlightHours: null,
        TaskGrades: []
    };

    // Agregar ViewType si existe
    const viewType = form.querySelector('input[name="ViewType"]');
    if (viewType && viewType.value === 'NonEvaluableMission') {
        formData.ViewType = viewType.value;
    }

    // Convertir fecha DD/MM/YYYY a ISO
    const dateValue = form.querySelector('input[name="EvaluationDate"]').value;
    const [day, month, year] = dateValue.split('/');
    formData.EvaluationDate = `${year}-${month}-${day}T00:00:00`;

    // Agregar horas de vuelo máquina si el instructor las ingresó
    const machineHoursInput = form.querySelector('input[name="MachineFlightHours"]');
    if (machineHoursInput && machineHoursInput.value.trim() !== '') {
        formData.MachineFlightHours = parseFloat(machineHoursInput.value);
    }

    // Recopilar calificaciones de tareas
    const taskContainers = form.querySelectorAll('.task-evaluation-card');
    taskContainers.forEach((container, index) => {
        const selectedGrade = container.querySelector(`input[name="TaskGrades[${index}].Grade"]:checked`);
        const taskIdInput = container.querySelector(`input[name="TaskGrades[${index}].TaskId"]`);

        if (selectedGrade && taskIdInput) {
            const taskGrade = {
                TaskId: taskIdInput.value,
                Grade: selectedGrade.value,
                NRedReasons: []
            };

            // Si es N o NR, recopilar razones
            if (selectedGrade.value === 'N' || selectedGrade.value === 'NR') {
                const nredContainer = container.querySelector('.nred-fields-container');
                const reasonInputs = nredContainer?.querySelectorAll('input[type="hidden"][name*="ReasonCategory"]');

                reasonInputs?.forEach(reasonInput => {
                    const category = reasonInput.value;
                    // Buscar el input de descripción correspondiente
                    const namePrefix = reasonInput.name.replace('.ReasonCategory', '');
                    const descriptionInput = nredContainer.querySelector(`input[name="${namePrefix}.ReasonDescription"]`);

                    if (descriptionInput) {
                        taskGrade.NRedReasons.push({
                            ReasonCategory: category,
                            ReasonDescription: descriptionInput.value
                        });
                    }
                });
            }

            formData.TaskGrades.push(taskGrade);
        }
    });

    return formData;
}


/**
 * Inicializa validaciones personalizadas
 */
function initializeCustomValidations() {

    // Validación para longitud de observaciones
    const observationsField = document.getElementById('GeneralObservations');
    if (observationsField) {
        observationsField.addEventListener('input', function () {
            if (this.value.length > 2000) {
                this.setCustomValidity('Las observaciones no pueden exceder 2000 caracteres.');
                this.classList.add('is-invalid');
            }
            else if (this.value.length < 20) {
                this.setCustomValidity('Agregue una observación.');
                this.classList.add('is-invalid');
            }
            else {
                this.setCustomValidity('');
                this.classList.remove('is-invalid');
            }
        });
    }
}

// ==========================================
// MODAL DE COMITÉ
// ==========================================

/**
 * Inicializa el modal de decisión de comité
 */
function initializeCommitteeModal() {
    const form = document.getElementById('formCommitteeDecision');
    if (!form) return;

    form.addEventListener('submit', async function (e) {
        e.preventDefault();

        if (!this.checkValidity()) {
            e.stopPropagation();
            this.classList.add('was-validated');
            return;
        }

        const formData = new FormData(this);
        const data = {
            CommitteeId: formData.get('CommitteeId'),
            LeaderId: formData.get('LeaderId'),
            ActaNumber: formData.get('ActaNumber'),
            Decision: formData.get('Decision'),
            DecisionObservations: formData.get('DecisionObservations')
        };

        try {
            const response = await fetch('/Instructor/Evaluation/SaveCommitteeDecision', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                },
                body: JSON.stringify(data)
            });

            const result = await response.json();

            if (result.success) {
                await Swal.fire({
                    icon: 'success',
                    title: 'Decisión Registrada',
                    text: result.message || 'La decisión del comité ha sido registrada exitosamente',
                    confirmButtonText: 'Aceptar'
                });

                window.location.reload();
            } else {
                await Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: result.message || 'No se pudo registrar la decisión',
                    confirmButtonText: 'Aceptar'
                });
            }
        } catch (error) {
            console.error('Error:', error);
            await Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Ocurrió un error al procesar la solicitud',
                confirmButtonText: 'Aceptar'
            });
        }
    });
}

// ==========================================
// BOTÓN PROMOVER A SIGUIENTE FASE
// ==========================================

/**
 * Inicializa el botón de promover a siguiente fase
 */
function initializePromotePhaseButton() {
    const btn = document.getElementById('btnPromotePhase');
    if (!btn) return;

    btn.addEventListener('click', async function () {
        const studentId = this.dataset.studentId;
        const courseId = this.dataset.courseId;
        const phaseId = this.dataset.phaseId;
        const leaderId = this.dataset.leaderId;
        const studentName = this.dataset.studentName;
        const phaseName = this.dataset.phaseName;

        const result = await Swal.fire({
            title: '¿Confirmar promoción?',
            html: `¿Está seguro de promover a <strong>${studentName}</strong> a la siguiente fase?<br><br>` +
                `<small class="text-muted">Fase actual: ${phaseName}</small>`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#2d7d43ff',
            cancelButtonColor: '#a13b3bff',
            confirmButtonText: 'Sí, promover',
            cancelButtonText: 'Cancelar'
        });

        if (result.isConfirmed) {
            try {
                const response = await fetch('/Instructor/Evaluation/PromoteStudentToNextPhase', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        StudentId: studentId,
                        CourseId: courseId,
                        PhaseId: phaseId,
                        LeaderId: leaderId
                    })
                });

                const data = await response.json();

                if (data.success) {
                    await Swal.fire({
                        icon: 'success',
                        title: 'Estudiante Promovido',
                        text: data.message,
                        confirmButtonText: 'Aceptar'
                    });

                    // Recargar la página para mostrar la nueva fase
                    window.location.reload();
                } else {
                    await Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: data.message,
                        confirmButtonText: 'Aceptar'
                    });
                }
            } catch (error) {
                console.error('Error:', error);
                await Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Ocurrió un error al procesar la solicitud',
                    confirmButtonText: 'Aceptar'
                });
            }
        }
    });
}

// ==========================================
// BOTÓN FINALIZAR Y APROBAR PROGRAMA
// ==========================================

/**
 * Inicializa el botón de finalizar y aprobar programa
 */
function initializeFinalizeApproveButton() {
    const btn = document.getElementById('btnFinalizeAndApproveCourse');
    if (!btn) return;

    btn.addEventListener('click', async function () {
        const studentId = this.dataset.studentId;
        const courseId = this.dataset.courseId;
        const phaseId = this.dataset.phaseId;
        const leaderId = this.dataset.leaderId;
        const studentName = this.dataset.studentName;

        const result = await Swal.fire({
            title: '¿Finalizar y Aprobar Programa?',
            html: `
                <p>Está a punto de finalizar y aprobar el programa del estudiante <strong>${studentName}</strong>.</p>
                <p class="text-success">El estudiante ha completado exitosamente todas las fases del programa.</p>
            `,
            icon: 'success',
            showCancelButton: true,
            confirmButtonColor: '#2d7d43ff',
            cancelButtonColor: '#a13b3bff',
            confirmButtonText: 'Sí, Finalizar y Aprobar',
            cancelButtonText: 'Cancelar'
        });

        if (result.isConfirmed) {
            try {
                const response = await fetch('/Instructor/Evaluation/FinalizeAndApproveCourse', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        StudentId: studentId,
                        CourseId: courseId,
                        PhaseId: phaseId,
                        LeaderId: leaderId
                    })
                });

                const data = await response.json();

                if (data.success) {
                    await Swal.fire({
                        icon: 'success',
                        title: '¡Curso Finalizado!',
                        text: data.message,
                        confirmButtonText: 'Aceptar'
                    });

                    window.location.href = `/Academic/Course/Details?id=${courseId}`;
                } else {
                    await Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: data.message,
                        confirmButtonText: 'Aceptar'
                    });
                }
            } catch (error) {
                console.error('Error:', error);
                await Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Ocurrió un error al procesar la solicitud',
                    confirmButtonText: 'Aceptar'
                });
            }
        }
    });
}

// ==========================================
// SWITCH DE MISIÓN EVALUABLE
// ==========================================

/**
 * Inicializa el switch de misión evaluable
 * Detecta automáticamente en qué vista estamos y redirige cuando cambia
 */
function initializeMissionEvaluableSwitch() {
    const missionEvaluableSwitch = document.getElementById('isMissionEvaluable');
    const missionEvaluableLabel = document.getElementById('missionEvaluableLabel');

    if (!missionEvaluableSwitch || !missionEvaluableLabel) {
        return;
    }

    // Detectar en qué vista estamos basándonos en el input hidden ViewType
    const form = document.getElementById('evaluationForm');
    if (!form) return;

    const viewTypeInput = form.querySelector('input[name="ViewType"]');
    const isNonEvaluableView = viewTypeInput && viewTypeInput.value === 'NonEvaluableMission';

    // Obtener parámetros del modelo
    const studentId = form.querySelector('input[name="StudentId"]')?.value;
    const courseId = form.querySelector('input[name="CourseId"]')?.value;

    missionEvaluableSwitch.addEventListener('change', function () {
        const isChecked = this.checked;
        missionEvaluableLabel.textContent = isChecked ? 'SI' : 'NO';

        // Determinar si necesitamos redirigir
        let shouldRedirect = false;
        let redirectUrl = '';
        let loadingMessage = '';

        if (isNonEvaluableView && isChecked) {
            // Estamos en vista NO evaluable y cambiamos a SI -> ir a vista evaluable
            redirectUrl = `/Instructor/Evaluation/EvaluateStudent?studentId=${studentId}&courseId=${courseId}`;
            loadingMessage = 'Cambiando a misión evaluable...';
            shouldRedirect = true;
        } else if (!isNonEvaluableView && !isChecked) {
            // Estamos en vista evaluable y cambiamos a NO -> ir a vista NO evaluable
            redirectUrl = `/Instructor/Evaluation/EvaluateStudent?studentId=${studentId}&courseId=${courseId}&viewType=NonEvaluableMission`;
            loadingMessage = 'Cambiando a misión no evaluable...';
            shouldRedirect = true;
        }

        // Redirigir si es necesario con spinner de carga
        if (shouldRedirect && redirectUrl) {
            // Mostrar spinner de carga
            Swal.fire({
                title: loadingMessage,
                html: 'Por favor espere...',
                allowOutsideClick: false,
                allowEscapeKey: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            // Redirigir después de un pequeño delay para que se vea el spinner
            setTimeout(() => {
                window.location.href = redirectUrl;
            }, 300);
        }
    });
}