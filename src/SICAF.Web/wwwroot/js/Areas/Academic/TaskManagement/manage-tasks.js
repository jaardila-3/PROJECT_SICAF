// ===========================================
// GESTIÓN DE TAREAS - DRAGULA + VANILLA JS
// ===========================================

document.addEventListener('DOMContentLoaded', function () {
    // VARIABLES Y CONFIGURACIÓN
    const tbody = document.getElementById('tasksSortable');
    let hasChanges = false;
    let hasNameChanges = false;

    // DRAG AND DROP CON DRAGULA
    const drake = dragula([tbody], {
        moves: function (el, container, handle) {
            // Solo permite arrastrar si clickean en el handle o sus hijos
            return handle.classList.contains('drag-handle') ||
                handle.closest('.drag-handle') !== null;
        }
    });

    drake.on('drop', function () {
        updateDisplayOrders();
        hasChanges = true;
    });

    // FUNCIONES DE ORDENAMIENTO
    function updateDisplayOrders() {
        const rows = tbody.querySelectorAll('tr');
        rows.forEach((row, index) => {
            const input = row.querySelector('.display-order-input');
            input.value = index + 1;
            row.setAttribute('data-task-index', index);
        });
    }

    // MANEJO DE CHECKBOX P3 Y DROPDOWN
    tbody.addEventListener('change', function (e) {
        if (e.target.classList.contains('p3-checkbox')) {
            const row = e.target.closest('tr');
            const select = row.querySelector('.p3-mission-select');

            if (e.target.checked) {
                select.disabled = false;
                // Seleccionar la primera misión por defecto si no hay ninguna seleccionada
                if (!select.value) {
                    const firstOption = select.querySelector('option:not([value=""])');
                    if (firstOption) {
                        select.value = firstOption.value;
                    }
                }
            } else {
                select.disabled = true;
                select.value = '';
            }

            hasChanges = true;
        }

        if (e.target.classList.contains('p3-mission-select')) {
            hasChanges = true;
        }
    });

    // MANEJO DE EDICIóN DE NOMBRES
    tbody.addEventListener('input', function (e) {
        if (e.target.classList.contains('task-name-input')) {
            const input = e.target;
            const originalName = input.dataset.originalName;
            const currentName = input.value;
            const row = input.closest('tr');
            const usageInfo = row.querySelector('.task-usage-info');

            if (currentName !== originalName) {
                usageInfo.style.display = 'block';
                hasChanges = true;
                hasNameChanges = true;
                updateNameChangeWarning();
            } else {
                usageInfo.style.display = 'none';
                updateNameChangeWarning();
            }
        }
    });

    function updateNameChangeWarning() {
        const warning = document.getElementById('nameChangeWarning');
        const list = document.getElementById('nameChangeList');
        list.innerHTML = '';

        let hasAnyNameChange = false;

        const nameInputs = document.querySelectorAll('.task-name-input');
        nameInputs.forEach(function (input) {
            const originalName = input.dataset.originalName;
            const currentName = input.value;
            const totalPhases = input.dataset.totalPhases;
            const totalMissions = input.dataset.totalMissions;

            if (currentName !== originalName) {
                hasAnyNameChange = true;
                const li = document.createElement('li');
                li.innerHTML = `
                    <strong>"${originalName}"</strong> → <strong>"${currentName}"</strong>
                    <span class="text-muted">(${totalPhases} fases, ${totalMissions} misiones)</span>
                `;
                list.appendChild(li);
            }
        });

        if (hasAnyNameChange) {
            warning.style.display = 'block';
            hasNameChanges = true;
        } else {
            warning.style.display = 'none';
            hasNameChanges = false;
        }
    }

    // BOTóN DE INFO DE USO
    tbody.addEventListener('click', function (e) {
        const btnUsageInfo = e.target.closest('.btn-usage-info');
        if (btnUsageInfo) {
            const row = btnUsageInfo.closest('tr');
            const input = row.querySelector('.task-name-input');
            const taskName = input.value;
            const totalPhases = input.dataset.totalPhases;
            const totalMissions = input.dataset.totalMissions;

            Swal.fire({
                icon: 'info',
                title: taskName,
                html: `Esta tarea se usa en <strong>${totalPhases}</strong> fase(s) y <strong>${totalMissions}</strong> misión(es).`,
                confirmButtonText: 'Entendido'
            });
        }
    });

    // GUARDAR CAMBIOS
    const btnSaveChanges = document.getElementById('btnSaveChanges');
    btnSaveChanges.addEventListener('click', async function () {
        if (!hasChanges) {
            Swal.fire({
                icon: 'info',
                title: 'Sin cambios',
                text: 'No hay cambios para guardar',
                timer: 2000,
                showConfirmButton: false,
                toast: true,
                position: 'top-end'
            });
            return;
        }

        // Confirmación si hay cambios de nombre
        if (hasNameChanges) {
            const confirmResult = await Swal.fire({
                icon: 'warning',
                title: 'Advertencia de cambio de nombres',
                html: 'Ha modificado nombres de tareas. Esto afectará a <strong>TODAS</strong> las fases y misiones que usen estas tareas.<br><br>¿Desea continuar?',
                showCancelButton: true,
                confirmButtonText: 'Sí, guardar cambios',
                cancelButtonText: 'Cancelar',
                reverseButtons: true
            });

            if (!confirmResult.isConfirmed) {
                return;
            }
        }

        const btn = btnSaveChanges;
        btn.disabled = true;
        btn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Guardando...';

        try {
            const phaseId = document.getElementById('PhaseId').value;
            const tasks = [];

            const rows = document.querySelectorAll('#tasksSortable tr');
            rows.forEach(function (row) {
                const taskId = row.dataset.taskId;
                const taskName = row.querySelector('.task-name-input').value;
                const displayOrder = parseInt(row.querySelector('.display-order-input').value);
                const isP3InPhase = row.querySelector('.p3-checkbox').checked;
                const p3MissionSelect = row.querySelector('.p3-mission-select');
                const p3StartingMission = isP3InPhase ? parseInt(p3MissionSelect.value) : null;

                tasks.push({
                    taskId: taskId,
                    name: taskName,
                    displayOrder: displayOrder,
                    isP3InPhase: isP3InPhase,
                    p3StartingFromMission: p3StartingMission
                });
            });

            const dto = {
                phaseId: phaseId,
                tasks: tasks
            };

            const antiForgeryToken = document.querySelector('input[name="__RequestVerificationToken"]').value;
            const saveUrl = btn.dataset.saveUrl; // Obtendremos la URL desde un data attribute

            const response = await fetch(saveUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': antiForgeryToken
                },
                body: JSON.stringify(dto)
            });

            const result = await response.json();

            if (result.success) {
                // Resetear flags para evitar la alerta de beforeunload
                hasChanges = false;
                hasNameChanges = false;

                Swal.fire({
                    icon: 'success',
                    title: '¡Éxito!',
                    text: result.message,
                    timer: 2000,
                    showConfirmButton: false,
                    toast: true,
                    position: 'top-end'
                });
                // Recargar después de 1 segundo
                setTimeout(() => location.reload(), 1000);
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error al guardar',
                    text: result.message || 'Error al guardar los cambios',
                    confirmButtonText: 'Entendido',
                });
                btn.disabled = false;
                btn.innerHTML = '<i class="ti ti-device-floppy me-1"></i>Guardar Cambios';
            }
        } catch (error) {
            console.error(error);
            Swal.fire({
                icon: 'error',
                title: 'Error de conexión',
                text: 'Error al comunicarse con el servidor. Por favor, intente nuevamente.',
                confirmButtonText: 'Entendido',
            });
            btn.disabled = false;
            btn.innerHTML = '<i class="ti ti-device-floppy me-1"></i>Guardar Cambios';
        }
    });

    // ADVERTENCIA AL SALIR CON CAMBIOS
    window.addEventListener('beforeunload', function (e) {
        if (hasChanges) {
            e.preventDefault();
            e.returnValue = '';
        }
    });
});
