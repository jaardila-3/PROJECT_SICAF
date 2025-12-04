// Theme Initialization
(function () {
    'use strict';

    // Configuración del tema 
    const themeName = 'PONAL_Green_Theme';

    // Función para inicializar el tema
    const initializeTheme = () => {
        // Establecer el tema por defecto
        document.documentElement.setAttribute('data-color-theme', themeName);

        // Guardar en localStorage para persistencia
        localStorage.setItem('color-theme', themeName);

        // Establecer tema claro por defecto si no hay uno guardado
        const savedTheme = localStorage.getItem('data-bs-theme');
        if (!savedTheme) {
            document.documentElement.setAttribute('data-bs-theme', 'light');
            localStorage.setItem('data-bs-theme', 'light');
        } else {
            document.documentElement.setAttribute('data-bs-theme', savedTheme);
        }
    };

    // Event Listeners
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initializeTheme);
    } else {
        // DOM ya está cargado
        initializeTheme();
    }

})();