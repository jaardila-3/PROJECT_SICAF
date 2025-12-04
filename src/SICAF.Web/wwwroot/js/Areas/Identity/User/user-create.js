// user-create.js

/**
 * Gestiona la visibilidad de los campos según los roles seleccionados.
 * @param {HTMLElement} form - El elemento del formulario que contiene los datos.
 */
function initRoleBasedFields(form) {
    const $rolesSelect = $('#SelectedRoleIds');
    if (!$rolesSelect.length) return;

    const aviationFields = document.getElementById('aviationFields');
    const courseField = document.getElementById('courseField');
    const pidInput = document.getElementById('PID');
    const positionInput = document.getElementById('FlightPosition');
    const aircraftSelect = document.getElementById('WingType');
    const courseSelect = document.getElementById('CourseId');

    const aviationRoles = JSON.parse(form.dataset.aviationRolesJson);
    const studentRoleName = 'Estudiante';

    function checkRoles() {
        const selectedTexts = $rolesSelect.find('option:selected').map((i, el) => $(el).text()).get();

        // Verificar roles de aviación
        const hasAviationRole = selectedTexts.some(roleText =>
            aviationRoles.some(aviationRole =>
                roleText.toLowerCase().includes(aviationRole.toLowerCase())
            )
        );

        // Verificar rol de estudiante
        const hasStudentRole = selectedTexts.some(roleText =>
            roleText.toLowerCase().includes(studentRoleName.toLowerCase())
        );

        // Mostrar/ocultar campos de aviación
        aviationFields.style.display = hasAviationRole ? 'flex' : 'none';
        pidInput.required = hasAviationRole;
        positionInput.required = hasAviationRole;
        aircraftSelect.required = hasAviationRole;

        // Mostrar/ocultar campo de programa
        if (courseField) {
            courseField.style.display = hasStudentRole ? 'flex' : 'none';
            if (courseSelect) {
                courseSelect.required = hasStudentRole;
            }
        }

        // Estudiante no puede tener otros roles
        if (hasStudentRole && selectedTexts.length > 1) {
            Swal.fire({
                icon: 'warning',
                title: 'Restricción de Rol',
                text: 'El rol Estudiante no puede combinarse con otros roles.',
                confirmButtonColor: '#198754'
            });

            // Deseleccionar todos excepto Estudiante
            $rolesSelect.val($rolesSelect.find('option').filter(function () {
                return $(this).text().toLowerCase().includes(studentRoleName.toLowerCase());
            }).val()).trigger('change');
        }

        // Limpiar campos si no son necesarios
        if (!hasAviationRole) {
            pidInput.value = '';
            $('#WingType').val('').trigger('change');
        }

        if (!hasStudentRole && courseSelect) {
            $('#CourseId').val('').trigger('change');
        }
    }

    $rolesSelect.on('change select2:select select2:unselect', () => setTimeout(checkRoles, 100));
    setTimeout(checkRoles, 500);
}

// Punto de entrada principal
document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('user-create-form');
    if (!form) {
        console.error('El formulario con ID "user-create-form" no fue encontrado.');
        return;
    }

    initRoleBasedFields(form);
});