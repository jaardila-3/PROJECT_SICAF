/**
 * assign-instructors.js
 * Asignación de instructores a un programa
 */

// Variables globales del módulo
let wingTypeSelect = null;
let instructorCheckboxes = [];
let assignForm = null;

/**
 * Inicializa el gestor de asignación de instructores
 */
function initAssignInstructors() {
    // Esperar a que el DOM esté listo
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', setupElements);
    } else {
        setupElements();
    }
}

/**
 * Configura los elementos y event listeners
 */
function setupElements() {
    // Obtener elementos del DOM
    wingTypeSelect = document.getElementById('WingType');
    assignForm = document.getElementById('formAssignInstructors');
    instructorCheckboxes = Array.from(document.querySelectorAll('input[name="InstructorIds"]'));

    // Configurar event listeners
    setupEventListeners();

    // Configurar estado inicial
    initializeView();
}

/**
 * Configura todos los event listeners
 */
function setupEventListeners() {
    // Filtro por tipo de ala
    if (wingTypeSelect) {
        wingTypeSelect.addEventListener('change', function (e) {
            filterInstructorsByWingType(e.target.value);
        });
    }

    // Validación del formulario
    if (assignForm) {
        assignForm.addEventListener('submit', function (e) {
            validateForm(e);
        });
    }

    // Listener para "Seleccionar todos"
    const selectAllBtn = document.getElementById('selectAllInstructors');
    if (selectAllBtn) {
        selectAllBtn.addEventListener('click', selectAllVisible);
    }

    // Listener para "Deseleccionar todos"
    const deselectAllBtn = document.getElementById('deselectAllInstructors');
    if (deselectAllBtn) {
        deselectAllBtn.addEventListener('click', deselectAll);
    }

    // Setup para radio buttons de líder
    setupLeaderRadioButtons();

    // Modificar el listener de checkboxes para validar el líder
    document.addEventListener('change', function (e) {
        if (e.target.name === 'InstructorIds') {
            const checkbox = e.target;
            if (!checkbox.checked) {
                // Si se desmarca un instructor que es líder, quitar la selección de líder
                const leaderId = checkbox.value;

                // Verificar si es líder de ala fija
                const leaderFixedRadio = document.getElementById('leaderFixed_' + leaderId);
                if (leaderFixedRadio && leaderFixedRadio.checked) {
                    leaderFixedRadio.checked = false;
                    const item = checkbox.closest('.instructor-item');
                    if (item) {
                        item.classList.remove('border-primary', 'bg-primary-subtle');
                    }
                }

                // Verificar si es líder de ala rotatoria
                const leaderRotaryRadio = document.getElementById('leaderRotary_' + leaderId);
                if (leaderRotaryRadio && leaderRotaryRadio.checked) {
                    leaderRotaryRadio.checked = false;
                    const item = checkbox.closest('.instructor-item');
                    if (item) {
                        item.classList.remove('border-info', 'bg-info-subtle');
                    }
                }
            }
            updateSelectionCounter();
        }
    });
}

/**
 * Listener para radio buttons de líder
 */
function setupLeaderRadioButtons() {
    document.addEventListener('change', function (e) {
        if (e.target.name === 'FlightLeaderFixedWingId' || e.target.name === 'FlightLeaderRotaryWingId') {
            handleLeaderRadioChange(e.target);
        }
    });
}

/**
 * Maneja el cambio en la selección del líder
 */
function handleLeaderRadioChange(radioButton) {
    if (radioButton.checked) {
        const instructorId = radioButton.value;
        const instructorCheckbox = document.getElementById('instructor_' + instructorId);
        const wingType = radioButton.name === 'FlightLeaderFixedWingId' ? 'FIJA' : 'ROTATORIA';

        // Automáticamente marcar el checkbox del instructor si se selecciona como líder
        if (instructorCheckbox && !instructorCheckbox.checked) {
            instructorCheckbox.checked = true;
            updateSelectionCounter();
        }

        // Limpiar resaltado previo del mismo tipo de ala
        document.querySelectorAll('.instructor-item').forEach(function (item) {
            const itemWingType = item.dataset.wingType;
            if (itemWingType === wingType) {
                if (wingType === 'FIJA') {
                    item.classList.remove('border-primary', 'bg-primary-subtle');
                } else {
                    item.classList.remove('border-info', 'bg-info-subtle');
                }
            }
        });

        // Resaltar visualmente el instructor líder con color específico por tipo de ala
        const leaderItem = radioButton.closest('.instructor-item');
        if (leaderItem) {
            if (wingType === 'FIJA') {
                leaderItem.classList.add('border-primary', 'bg-primary-subtle');
            } else {
                leaderItem.classList.add('border-info', 'bg-info-subtle');
            }
        }
    }
}

/**
 * Inicializa la vista con configuraciones por defecto
 */
function initializeView() {
    updateSelectionCounter();

    // Si hay un tipo de ala preseleccionado, aplicar filtro
    if (wingTypeSelect && wingTypeSelect.value) {
        filterInstructorsByWingType(wingTypeSelect.value);
    }
}

/**
 * Filtra instructores por tipo de ala
 * @param {string} selectedType - Tipo de ala seleccionado
 */
function filterInstructorsByWingType(selectedType) {
    const instructorItems = document.querySelectorAll('.form-check');
    let visibleCount = 0;

    instructorItems.forEach(function (item) {
        const aircraftBadge = item.querySelector('.badge.bg-secondary');
        const checkbox = item.querySelector('input[type="checkbox"]');

        if (!aircraftBadge) return;

        const instructorWingType = aircraftBadge.textContent.trim();

        // Mostrar si no hay filtro, coincide el tipo, o es N/A
        const shouldShow = selectedType === '' ||
            instructorWingType === selectedType ||
            instructorWingType === 'N/A';

        if (shouldShow) {
            showElement(item);
            visibleCount++;
        } else {
            hideElement(item);
            // Desmarcar checkbox si se oculta
            if (checkbox) {
                checkbox.checked = false;
            }
        }
    });

    // Actualizar contador de instructores
    updateVisibleCounter(visibleCount);
    updateSelectionCounter();
}

/**
 * Muestra un elemento con animación suave
 * @param {HTMLElement} element - Elemento a mostrar
 */
function showElement(element) {
    element.style.display = '';
    element.style.opacity = '0';
    element.style.transform = 'translateY(-10px)';

    // Animación de entrada
    requestAnimationFrame(function () {
        element.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
        element.style.opacity = '1';
        element.style.transform = 'translateY(0)';
    });
}

/**
 * Oculta un elemento con animación suave
 * @param {HTMLElement} element - Elemento a ocultar
 */
function hideElement(element) {
    element.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
    element.style.opacity = '0';
    element.style.transform = 'translateY(-10px)';

    setTimeout(function () {
        element.style.display = 'none';
    }, 300);
}

/**
 * Selecciona todos los instructores
 */
function selectAllVisible() {
    const visibleCheckboxes = getVisibleCheckboxes();

    visibleCheckboxes.forEach(function (checkbox) {
        checkbox.checked = true;
    });

    updateSelectionCounter();
    showMessage('Todos los instructores han sido seleccionados', 'info');
}

/**
 * Deselecciona todos los instructores
 */
function deselectAll() {
    instructorCheckboxes.forEach(function (checkbox) {
        checkbox.checked = false;
    });

    updateSelectionCounter();
    showMessage('Selección limpiada', 'info');
}

/**
 * Obtiene los checkboxes 
 * @returns {HTMLInputElement[]} Array de checkboxes 
 */
function getVisibleCheckboxes() {
    return instructorCheckboxes.filter(function (checkbox) {
        const item = checkbox.closest('.form-check');
        return item && item.style.display !== 'none';
    });
}

/**
 * Actualiza el contador de selección
 * Esta función ahora maneja correctamente el botón submit
 */
function updateSelectionCounter() {
    const selectedCount = instructorCheckboxes.filter(function (cb) {
        return cb.checked;
    }).length;

    const totalVisible = getVisibleCheckboxes().length;

    // Actualizar elementos de contador si existen
    const counterElement = document.getElementById('selectionCounter');
    if (counterElement) {
        counterElement.textContent = selectedCount + ' de ' + totalVisible + ' seleccionados';

        // Cambiar estilo según selección
        if (selectedCount > 0) {
            counterElement.className = 'badge bg-success';
        } else {
            counterElement.className = 'badge bg-secondary';
        }
    }
}

/**
 * Actualiza el contador de instructores 
 * @param {number} count - Número de instructores 
 */
function updateVisibleCounter(count) {
    const visibleCountElement = document.getElementById('visibleCounter');
    if (visibleCountElement) {
        const instructorText = count === 1 ? 'instructor' : 'instructores';
        const disponibleText = count === 1 ? 'disponible' : 'disponibles';
        visibleCountElement.textContent = count + ' ' + instructorText + ' ' + disponibleText;
    }
}

/**
 * Valida el formulario antes del envío
 * @param {Event} e - Evento de submit
 */
function validateForm(e) {
    const checkedBoxes = instructorCheckboxes.filter(function (cb) {
        return cb.checked;
    });

    if (checkedBoxes.length === 0) {
        e.preventDefault();
        showMessage('Debe seleccionar al menos un instructor', 'warning');
        return false;
    }
    return true;
}

/**
 * Muestra un mensaje personalizado
 * @param {string} message - Mensaje a mostrar
 * @param {string} type - Tipo de mensaje (info, warning, success, error)
 */
function showMessage(message, type) {
    if (typeof type === 'undefined') type = 'info';

    if (typeof Swal !== 'undefined') {
        Swal.fire({
            icon: type,
            text: message,
            timer: 2000,
            showConfirmButton: false,
            toast: true,
            position: 'top-end'
        });
    }
}

// Inicializar automáticamente cuando el script se carga
initAssignInstructors();