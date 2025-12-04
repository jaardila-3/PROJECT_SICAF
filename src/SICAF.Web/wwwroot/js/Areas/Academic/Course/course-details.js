/**
 * Gestión de detalles de programa
 * Sistema SICAF - Módulo Académico
 */

// Variables globales del módulo
let updateEndDateForm = null;
let modalInstance = null;
let courseId = null;
let courseStartDate = null;

/**
 * Inicializa la gestión de detalles del programa
 */
function initCourseDetails() {
    // Esperar a que el DOM esté listo
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', setupCourseDetails);
    } else {
        setupCourseDetails();
    }
}

/**
 * Configura los elementos y funcionalidades del programa
 */
function setupCourseDetails() {
    // Obtener elementos del DOM
    updateEndDateForm = document.getElementById('updateEndDateForm');

    if (updateEndDateForm) {
        // Obtener datos del programa desde atributos data
        courseId = updateEndDateForm.dataset.courseId;
        courseStartDate = updateEndDateForm.dataset.courseStartDate;

        setupEndDateModal();
        setupEventListeners();
    }
}

/**
 * Configura el modal de fecha de finalización
 */
function setupEndDateModal() {
    const modal = document.getElementById('editEndDateModal');
    if (modal) {
        modalInstance = new bootstrap.Modal(modal);

        // Configurar DatePicker si jQuery está disponible
        const datePickerElement = document.getElementById('newEndDate');
        if (datePickerElement && typeof $ !== 'undefined') {
            $(datePickerElement).bootstrapMaterialDatePicker({
                weekStart: 0,
                time: false
            });
        }
    }
}

/**
 * Configura los event listeners
 */
function setupEventListeners() {
    if (updateEndDateForm) {
        updateEndDateForm.addEventListener('submit', handleUpdateEndDate);
    }
}

/**
 * Maneja la actualización de fecha de finalización
 * @param {Event} e - Evento de submit
 */
function handleUpdateEndDate(e) {
    e.preventDefault();

    const newEndDate = document.getElementById('newEndDate').value;

    // Validar la nueva fecha de finalización
    if (!validateEndDate(newEndDate)) {
        return;
    }

    // Construir URL para la petición
    const baseUrl = updateEndDateForm.dataset.updateUrl || '/Academic/Course/UpdateEndDate';
    const url = baseUrl + '?courseId=' + encodeURIComponent(courseId) + '&newEndDate=' + encodeURIComponent(newEndDate);

    fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {

            if (result.success) {
                handleUpdateSuccess(newEndDate, result.message);
            } else {
                showErrorMessage('Error', result.message);
            }
        })
        .catch(function (error) {
            console.error('Error:', error);
            showErrorMessage('Error', 'Error al actualizar la fecha');
        });
}

/**
 * Valida la fecha de finalización
 * @param {string} newEndDate - Nueva fecha a validar
 * @returns {boolean} True si es válida
 */
function validateEndDate(newEndDate) {
    if (!newEndDate) {
        showErrorMessage('Error', 'Debe seleccionar una fecha de finalización');
        return false;
    }

    if (new Date(newEndDate) <= new Date(courseStartDate)) {
        showErrorMessage(
            'Error',
            'La fecha de finalización debe ser posterior a la fecha de inicio del programa.'
        );
        return false;
    }

    return true;
}

/**
 * Maneja el éxito de la actualización
 * @param {string} newEndDate - Nueva fecha
 * @param {string} message - Mensaje de éxito
 */
function handleUpdateSuccess(newEndDate, message) {
    // Actualizar la fecha en la vista
    const endDateDisplay = document.getElementById('endDateDisplay');
    if (endDateDisplay) {
        endDateDisplay.textContent = new Date(newEndDate).toLocaleDateString('es-ES');
    }

    // Cerrar modal
    if (modalInstance) {
        modalInstance.hide();
    }

    // Mostrar mensaje de éxito
    showSuccessMessage('Éxito', message)
        .then(function () {
            // Recargar página para mostrar cambios
            window.location.reload();
        });
}

/**
 * Muestra un mensaje de error
 * @param {string} title - Título del mensaje
 * @param {string} text - Texto del mensaje
 */
function showErrorMessage(title, text) {
    if (typeof Swal !== 'undefined') {
        Swal.fire({
            icon: 'error',
            title: title,
            text: text
        });
    } else {
        alert(title + ': ' + text);
    }
}

/**
 * Muestra un mensaje de éxito
 * @param {string} title - Título del mensaje
 * @param {string} text - Texto del mensaje
 * @returns {Promise} Promise que se resuelve cuando se cierra
 */
function showSuccessMessage(title, text) {
    if (typeof Swal !== 'undefined') {
        return Swal.fire({
            icon: 'success',
            title: title,
            text: text,
            timer: 3000,
            showConfirmButton: false
        });
    }
}

// Inicializar automáticamente cuando el script se carga
initCourseDetails();