using Microsoft.Extensions.Options;

using SICAF.Common.Configuration.Options;
using SICAF.Web.Interfaces.Files;
using SICAF.Web.Models;

namespace SICAF.Web.Services.Files;

/// <summary>
/// Servicio para validación de archivos de imagen
/// </summary>
public class ImageValidationService(
    IOptions<ImageValidationOptions> options,
    ILogger<ImageValidationService>? logger = null
    ) : IImageValidationService
{
    private readonly ImageValidationOptions _options = options?.Value ?? new ImageValidationOptions();
    private readonly ILogger<ImageValidationService>? _logger = logger;

    /// <summary>
    /// Valida un archivo de imagen y retorna los bytes si es válido
    /// </summary>
    public async Task<ImageValidationResult> ValidateAndProcessImageAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return new ImageValidationResult
            {
                IsValid = false,
                ErrorMessage = "No se ha seleccionado ningún archivo."
            };
        }

        // Validar tamaño
        if (file.Length > _options.MaxFileSizeBytes)
        {
            var sizeMB = _options.MaxFileSizeBytes / (1024.0 * 1024.0);
            var fileSizeMB = file.Length / (1024.0 * 1024.0);
            return new ImageValidationResult
            {
                IsValid = false,
                ErrorMessage = $"El archivo excede el tamaño máximo permitido de {sizeMB:F1} MB. Tamaño actual: {fileSizeMB:F1} MB."
            };
        }

        // Validar extensión
        var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !_options.AllowedExtensions.Contains(extension))
        {
            return new ImageValidationResult
            {
                IsValid = false,
                ErrorMessage = $"Tipo de archivo no permitido. Solo se permiten: {string.Join(", ", _options.AllowedExtensions)}"
            };
        }

        // Validar MIME type
        if (!_options.AllowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
        {
            return new ImageValidationResult
            {
                IsValid = false,
                ErrorMessage = "El tipo MIME del archivo no es válido para una imagen."
            };
        }

        // Leer el archivo en memoria
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        byte[] imageData = memoryStream.ToArray();

        // Validar que realmente sea una imagen verificando los magic bytes
        if (!IsValidImageFormat(imageData))
        {
            return new ImageValidationResult
            {
                IsValid = false,
                ErrorMessage = "El archivo no es una imagen válida o está corrupto."
            };
        }

        return new ImageValidationResult
        {
            IsValid = true,
            ImageData = imageData,
            ContentType = file.ContentType,
            FileName = file.FileName
        };
    }

    /// <summary>
    /// Verifica los magic bytes de los formatos de imagen comunes
    /// </summary>
    private bool IsValidImageFormat(byte[] bytes)
    {
        if (bytes == null || bytes.Length < 4)
            return false;

        // JPEG
        if (bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
            return true;

        // PNG
        if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
            return true;

        // BMP
        if (bytes[0] == 0x42 && bytes[1] == 0x4D)
            return true;

        // WebP
        if (bytes.Length >= 12)
        {
            var webpHeader = System.Text.Encoding.ASCII.GetString(bytes, 0, 4);
            var webpType = System.Text.Encoding.ASCII.GetString(bytes, 8, 4);
            if (webpHeader == "RIFF" && webpType == "WEBP")
                return true;
        }

        return false;
    }

}