/**
 * Gestión de instructores de programa
 * Sistema SICAF - Módulo Académico
 */
class CourseInstructorsManager {
    constructor() {
        this.courseId = null;
        this.baseUrl = '/Academic/Course';
        this.instructorsContainer = null;

        this.init();
    }

    /**
     * Inicializa el gestor de instructores
     */
    init() {
        // Esperar a que el DOM esté listo
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', () => this.setupInstructorsTable());
        } else {
            this.setupInstructorsTable();
        }
    }

    /**
     * Configura la tabla de instructores si existe en la página
     */
    setupInstructorsTable() {
        this.instructorsContainer = document.getElementById('instructorsTable');

        if (this.instructorsContainer) {
            this.courseId = this.instructorsContainer.dataset.courseId;

            if (this.courseId) {
                this.loadCourseInstructors();
                this.setupEventListeners();
            }
        }
    }

    /**
     * Configura los event listeners
     */
    setupEventListeners() {
        // Listener para formularios de desasignación (delegación de eventos)
        document.addEventListener('submit', (e) => {
            if (e.target.classList.contains('unassign-instructor-form')) {
                e.preventDefault();
                this.handleUnassignInstructor(e.target);
            }
        });
    }

    /**
     * Carga los instructores del programa via AJAX
     */
    async loadCourseInstructors() {
        try {
            const response = await fetch(`${this.baseUrl}/GetCourseInstructors?courseId=${this.courseId}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();

            if (result.success) {
                this.renderInstructorsTable(result.data);
            } else {
                this.showError('Error: ' + result.message);
            }
        } catch (error) {
            console.error('Error loading instructors:', error);
            this.showError('Error al cargar los instructores');
        }
    }

    /**
     * Renderiza la tabla de instructores
     * @param {Array} instructors - Lista de instructores
     */
    renderInstructorsTable(instructors) {
        if (!instructors || instructors.length === 0) {
            this.instructorsContainer.innerHTML = `
                <div class="alert alert-info text-center">
                    <i class="ti ti-info-circle me-2"></i>
                    No hay instructores asignados a este programa.
                </div>
            `;
            return;
        }

        const tableHtml = `
            <div class="table-responsive">
                <table id="instructorsDataTable" class="table table-hover">
                    <thead>
                        <tr>
                            <th class="text-center">Identificación</th>
                            <th class="text-center">Instructor</th>
                            <th class="text-center">Rol</th>
                            <th class="text-center">Ala</th>
                            <th class="text-center no-export">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${instructors.map(instructor => this.createInstructorRow(instructor)).join('')}
                    </tbody>
                </table>
            </div>
        `;

        this.instructorsContainer.innerHTML = tableHtml;

        // Inicializar DataTable usando la función global de SICAF
        if (typeof window.initSICAFDataTable === 'function') {
            window.initSICAFDataTable('#instructorsDataTable', {
                title: 'Instructores del programa - SICAF',
                filename: 'SICAF_Instructores_' + new Date().toISOString().slice(0, 10)
            });
        }
    }

    /**
     * Crea una fila de instructor
     * @param {Object} instructor - Datos del instructor
     * @returns {string} HTML de la fila
     */
    createInstructorRow(instructor) {
        const badgeColor = instructor.participationType === 'Líder de vuelo' ? 'bg-primary' : 'bg-secondary';
        const aircraftBadge = instructor.wingType ?
            `<span class="badge bg-success">${instructor.wingType}</span>` :
            '<span class="badge bg-light text-dark">N/A</span>';

        //const formattedDate = new Date(instructor.assignmentDate).toLocaleDateString('es-CO');

        return `
            <tr>
                <td class="text-center">${this.escapeHtml(instructor.instructorIdentification)}</td>
                <td class="text-center">${this.escapeHtml(instructor.instructorName)}</td>
                <td class="text-center"><span class="badge ${badgeColor}">${this.escapeHtml(instructor.participationType)}</span></td>
                <td class="text-center">${aircraftBadge}</td>
                <td class="text-center">

                    <a href="/Identity/User/Details?id=${instructor.instructorId}&courseId=${this.courseId}" 
                        class="btn btn-sm btn-info" title="Ver Instructor">
                        <i class="ti ti-user"></i>
                    </a>

                    <form method="post" action="${this.baseUrl}/UnassignInstructor" 
                          class="unassign-instructor-form d-inline">
                        <input type="hidden" name="courseId" value="${this.courseId}" />
                        <input type="hidden" name="instructorId" value="${instructor.instructorId}" />
                        <input type="hidden" name="__RequestVerificationToken" value="${this.getAntiForgeryToken()}" />
                        <button type="submit" class="btn btn-sm btn-danger" 
                                title="Desasignar instructor">
                            <i class="ti ti-user-minus"></i>
                        </button>
                    </form>
                </td>
            </tr>
        `;
    }

    /**
     * Maneja la desasignación de instructor
     * @param {HTMLFormElement} form - Formulario de desasignación
     */
    async handleUnassignInstructor(form) {
        const confirmed = await this.showConfirmDialog(
            '¿Está seguro de desasignar este instructor?',
            'Esta acción no se puede deshacer'
        );

        if (!confirmed) return;

        try {
            const formData = new FormData(form);

            const response = await fetch(form.action, {
                method: 'POST',
                body: formData,
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });

            if (response.redirected) {
                // Si hay redirección, recargar la página para mostrar mensajes
                window.location.reload();
                return;
            }

            const result = await response.json();

            if (result.success) {
                await this.showSuccessMessage('Instructor desasignado exitosamente');
                this.loadCourseInstructors(); // Recargar tabla
            } else {
                this.showError(result.message || 'Error al desasignar instructor');
            }
        } catch (error) {
            console.error('Error unassigning instructor:', error);
            this.showError('Error al procesar la solicitud');
        }
    }

    /**
     * Muestra un mensaje de error
     * @param {string} message - Mensaje de error
     */
    showError(message) {
        this.instructorsContainer.innerHTML = `
            <div class="alert alert-danger">
                <i class="ti ti-alert-circle me-2"></i>
                ${this.escapeHtml(message)}
            </div>
        `;
    }

    /**
     * Muestra un diálogo de confirmación
     * @param {string} title - Título del diálogo
     * @param {string} text - Texto del diálogo
     * @returns {Promise<boolean>} Resultado de la confirmación
     */
    async showConfirmDialog(title, text) {

        if (typeof Swal !== 'undefined') {
            const result = await Swal.fire({
                title: title,
                text: text,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Sí, desasignar',
                cancelButtonText: 'Cancelar'
            });
            return result.isConfirmed;
        }

        // Fallback a confirm nativo
        return confirm(`${title}\n${text}`);
    }

    /**
     * Muestra un mensaje de éxito
     * @param {string} message - Mensaje de éxito
     */
    async showSuccessMessage(message) {
        if (typeof Swal !== 'undefined') {
            await Swal.fire({
                icon: 'success',
                title: 'Éxito',
                text: message,
                timer: 3000,
                showConfirmButton: false
            });
        } else {
            alert(message);
        }
    }

    /**
     * Obtiene el token anti-falsificación generado automaticamente por dotnet cuando se usa el tag <form>
     * @returns {string} Token CSRF
     */
    getAntiForgeryToken() {
        const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
        return tokenInput ? tokenInput.value : '';
    }

    /**
     * Escapa HTML para prevenir XSS
     * @param {string} unsafe - Texto sin escapar
     * @returns {string} Texto escapado
     */
    escapeHtml(unsafe) {
        if (typeof unsafe !== 'string') return '';

        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }
}

// Inicializar automáticamente cuando el script se carga
new CourseInstructorsManager();