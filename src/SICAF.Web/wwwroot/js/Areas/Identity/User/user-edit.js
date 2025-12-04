// user-edit.js

// Función para inicializar el toggle de bloqueo de cuenta
function initLockoutToggle(form) {
    const lockoutSwitch = document.getElementById('lockoutSwitch');
    if (!lockoutSwitch) return;

    const lockoutDateContainer = document.getElementById('lockoutDateContainer');
    const lockoutReasonContainer = document.getElementById('lockoutReasonContainer');
    const lockoutLabel = document.getElementById('lockoutLabel');
    const lockoutDateInput = document.getElementById('lockoutDate');
    const lockoutReasonTextarea = document.getElementById('lockoutReasonTextarea');

    // validar bloqueo desde el atributo data-* del formulario
    const isCurrentlyLocked = form.dataset.isLockedOut === 'true';

    function updateSwitchStyle(wantsToLock) {
        const switchElement = lockoutSwitch.parentElement;
        switchElement.classList.toggle('switch-danger', wantsToLock);
        switchElement.classList.toggle('switch-success', !wantsToLock);
        lockoutLabel.classList.toggle('text-danger', wantsToLock);
        lockoutLabel.classList.toggle('text-success', !wantsToLock);

        if (wantsToLock) {
            lockoutLabel.textContent = 'Bloquear Usuario';
        } else {
            lockoutLabel.textContent = isCurrentlyLocked ? 'Desbloquear Usuario' : 'Usuario Activo';
        }
    }

    function updateFieldsVisibility(show) {
        lockoutDateContainer.style.display = show ? 'block' : 'none';
        lockoutReasonContainer.style.display = show ? 'block' : 'none';
    }

    lockoutSwitch.addEventListener('change', function () {
        const wantsToLock = this.checked;
        updateSwitchStyle(wantsToLock);
        updateFieldsVisibility(wantsToLock);

        if (wantsToLock && !isCurrentlyLocked) {
            lockoutReasonTextarea.removeAttribute('readonly');
            if (!lockoutReasonTextarea.value && !lockoutDateInput.value) {
                lockoutReasonTextarea.value = '';
                lockoutDateInput.value = '';
            }
            lockoutReasonTextarea.focus();
        }
    });

    // Estado inicial del switch
    const initialWantToLock = lockoutSwitch.checked;
    updateSwitchStyle(initialWantToLock);
    updateFieldsVisibility(initialWantToLock);

    if (initialWantToLock && !isCurrentlyLocked) {
        lockoutReasonTextarea.removeAttribute('readonly');
    }
}

// Función para inicializar la lógica de los campos de aviación
function initAviationFields(form) {
    const $rolesSelect = $('#SelectedRoleIds');
    if (!$rolesSelect.length) return;

    const aviationFields = document.getElementById('aviationFields');
    const pidInput = document.getElementById('PID');
    const positionInput = document.getElementById('FlightPosition');
    const aircraftSelect = document.getElementById('WingType');
    const courseField = document.getElementById('courseField');
    const courseSelect = document.getElementById('CourseId');
    const studentRoleName = 'Estudiante';

    const hasAviationProfile = form.dataset.hasAviationProfile === 'true';
    const aviationRoles = JSON.parse(form.dataset.aviationRolesJson); // Parseamos el JSON
    const currentRoles = JSON.parse(form.dataset.currentRoles);

    function checkAviationRoles() {
        const selectedTexts = $rolesSelect.find('option:selected').map((i, el) => $(el).text()).get();

        const hasAviationRole = selectedTexts.some(roleText =>
            aviationRoles.some(aviationRole =>
                roleText.toLowerCase().includes(aviationRole.toLowerCase())
            )
        );

        // Verificar rol de estudiante
        const hasStudentRole = selectedTexts.some(roleText =>
            roleText.toLowerCase().includes(studentRoleName.toLowerCase())
        );

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

        // Validar restricción del rol Estudiante
        if (hasStudentRole && selectedTexts.length > 1) {
            Swal.fire({
                icon: 'warning',
                title: 'Restricción de Rol',
                text: 'El rol Estudiante no puede combinarse con otros roles.',
                confirmButtonColor: '#198754'
            });

            $rolesSelect.val($rolesSelect.find('option').filter(function () {
                return $(this).text().toLowerCase().includes(studentRoleName.toLowerCase());
            }).val()).trigger('change');
        }

        if (!hasAviationRole) {
            pidInput.value = '';
            $('#WingType').val('').trigger('change');
        }

        // Limpiar programa si no es estudiante
        if (!hasStudentRole && courseSelect) {
            $('#CourseId').val('').trigger('change');
        } else {
            // Si tiene rol de estudiante y ya tiene aviationProfile, restaurar valores
            if (hasAviationRole && hasAviationProfile && !pidInput.value && form.dataset.pid) {
                pidInput.value = form.dataset.pid;
                $('#WingType').val(form.dataset.wingType).trigger('change');
            }
        }
    }

    $rolesSelect.on('change select2:select select2:unselect', () => setTimeout(checkAviationRoles, 100));

    // Comprobación inicial
    if (currentRoles.some(role => aviationRoles.includes(role))) {
        aviationFields.style.display = 'flex';
    }
    setTimeout(checkAviationRoles, 500); // Dar tiempo a que Select2 se inicialice
}

// Punto de entrada principal: Se ejecuta cuando el DOM está listo.
document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('user-edit-form');
    if (!form) {
        console.error('El formulario con ID "user-edit-form" no fue encontrado.');
        return;
    }

    // Inicializar Date Pickers
    $("#birthdatePicker").bootstrapMaterialDatePicker({ weekStart: 0, time: false, currentDate: '2020-01-01' });
    $("#lockoutDate").bootstrapMaterialDatePicker({ weekStart: 0, time: false });

    // Inicializar módulos de lógica
    initLockoutToggle(form);
    initAviationFields(form);
});