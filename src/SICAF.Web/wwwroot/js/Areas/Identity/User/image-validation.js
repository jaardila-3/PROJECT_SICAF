// Módulo de validación de imágenes
export class ImageValidator {
    constructor(options = {}) {
        this.maxSizeMB = options.maxSizeMB || 2;
        this.maxSizeBytes = this.maxSizeMB * 1024 * 1024;
        this.allowedExtensions = options.allowedExtensions || ['.jpg', '.jpeg', '.png', '.webp'];
        this.allowedMimeTypes = options.allowedMimeTypes || [
            'image/jpeg',
            'image/jpg',
            'image/png',
            'image/bmp',
            'image/webp'
        ];
        this.maxWidth = options.maxWidth || 800;
        this.maxHeight = options.maxHeight || 800;
    }

    /**
     * Valida un archivo de imagen
     * @param {File} file - El archivo a validar
     * @returns {Promise<{isValid: boolean, error?: string, preview?: string}>}
     */
    async validateImage(file) {
        if (!file) {
            return { isValid: false, error: 'No se ha seleccionado ningún archivo.' };
        }

        // Validar tamaño
        if (file.size > this.maxSizeBytes) {
            const fileSizeMB = (file.size / (1024 * 1024)).toFixed(1);
            return {
                isValid: false,
                error: `El archivo excede el tamaño máximo de ${this.maxSizeMB} MB. Tamaño actual: ${fileSizeMB} MB.`
            };
        }

        // Validar extensión
        const fileName = file.name.toLowerCase();
        const extension = fileName.substring(fileName.lastIndexOf('.'));
        if (!this.allowedExtensions.includes(extension)) {
            return {
                isValid: false,
                error: `Tipo de archivo no permitido. Solo se permiten: ${this.allowedExtensions.join(', ')}`
            };
        }

        // Validar MIME type
        if (!this.allowedMimeTypes.includes(file.type.toLowerCase())) {
            return {
                isValid: false,
                error: 'El tipo de archivo no corresponde a una imagen válida.'
            };
        }

        // Validar dimensiones
        try {
            const dimensions = await this.getImageDimensions(file);
            if (dimensions.width > this.maxWidth || dimensions.height > this.maxHeight) {
                return {
                    isValid: false,
                    error: `Las dimensiones de la imagen (${dimensions.width}x${dimensions.height}) exceden el máximo permitido de ${this.maxWidth}x${this.maxHeight} píxeles.`
                };
            }

            // Crear preview
            const preview = await this.createPreview(file);

            return {
                isValid: true,
                preview: preview,
                dimensions: dimensions,
                sizeMB: (file.size / (1024 * 1024)).toFixed(2)
            };
        } catch (error) {
            return {
                isValid: false,
                error: 'Error al procesar la imagen. Asegúrese de que el archivo no esté corrupto.'
            };
        }
    }

    /**
     * Obtiene las dimensiones de una imagen
     * @param {File} file 
     * @returns {Promise<{width: number, height: number}>}
     */
    getImageDimensions(file) {
        return new Promise((resolve, reject) => {
            const img = new Image();
            img.onload = () => {
                resolve({ width: img.width, height: img.height });
            };
            img.onerror = reject;
            img.src = URL.createObjectURL(file);
        });
    }

    /**
     * Crea una vista previa de la imagen
     * @param {File} file 
     * @returns {Promise<string>}
     */
    createPreview(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onload = (e) => resolve(e.target.result);
            reader.onerror = reject;
            reader.readAsDataURL(file);
        });
    }

    /**
     * Muestra el resultado de la validación en la UI
     * @param {HTMLElement} container - Contenedor donde mostrar el resultado
     * @param {Object} result - Resultado de la validación
     */
    displayResult(container, result) {
        // Limpiar contenedor
        container.innerHTML = '';

        if (result.isValid) {
            // Mostrar preview
            const img = document.createElement('img');
            img.src = result.preview;
            img.className = ''; //'rounded-circle';
            img.style.width = '150px';
            img.style.height = '150px';
            img.style.objectFit = 'cover';
            container.appendChild(img);

            // Mostrar información
            const info = document.createElement('div');
            info.className = 'mt-2 small text-muted';
            info.innerHTML = `
                <div>Tamaño: ${result.sizeMB} MB</div>
                <div>Dimensiones: ${result.dimensions.width}x${result.dimensions.height}px</div>
            `;
            container.appendChild(info);
        } else {
            // Mostrar error
            const alert = document.createElement('div');
            alert.className = 'alert alert-danger';
            alert.innerHTML = `
                <i class="ti ti-alert-circle me-2"></i>
                ${result.error}
            `;
            container.appendChild(alert);

            // Mostrar placeholder
            const placeholder = document.createElement('div');
            placeholder.className = 'rounded-circle bg-secondary d-flex align-items-center justify-content-center mx-auto mt-3';
            placeholder.style.width = '150px';
            placeholder.style.height = '150px';
            placeholder.innerHTML = '<i class="ti ti-user fs-1 text-white"></i>';
            container.appendChild(placeholder);
        }
    }
}

// Función de inicialización
export function initializeImageValidation(inputId, previewId) {
    const input = document.getElementById(inputId);
    const preview = document.getElementById(previewId);

    if (!input || !preview) {
        console.error('No se encontraron los elementos necesarios');
        return;
    }

    const validator = new ImageValidator();

    input.addEventListener('change', async (e) => {
        const file = e.target.files[0];

        if (file) {
            // Mostrar loading
            preview.innerHTML = '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Validando...</span></div>';

            // Validar
            const result = await validator.validateImage(file);

            // Mostrar resultado
            validator.displayResult(preview, result);

            // Si no es válido, limpiar el input
            if (!result.isValid) {
                input.value = '';

                // Mostrar mensaje de error con SweetAlert si está disponible
                if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        icon: 'error',
                        title: 'Imagen no válida',
                        text: result.error,
                        confirmButtonColor: '#dc3545'
                    });
                }
            }
        } else {
            // Restaurar placeholder si no hay archivo
            preview.innerHTML = `
                <div class="rounded-circle bg-secondary d-flex align-items-center justify-content-center mx-auto" 
                     style="width: 150px; height: 150px;">
                    <i class="ti ti-user fs-1 text-white"></i>
                </div>
            `;
        }
    });

    // Agregar validación al enviar el formulario
    const form = input.closest('form');
    if (form) {
        form.addEventListener('submit', async (e) => {
            const file = input.files[0];
            if (file) {
                const result = await validator.validateImage(file);
                if (!result.isValid) {
                    e.preventDefault();

                    if (typeof Swal !== 'undefined') {
                        Swal.fire({
                            icon: 'error',
                            title: 'No se puede enviar el formulario',
                            text: result.error,
                            confirmButtonColor: '#dc3545'
                        });
                    } else {
                        alert(result.error);
                    }
                }
            }
        });
    }
}