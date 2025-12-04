using SICAF.Web.Models;

namespace SICAF.Web.Interfaces.Files;

public interface IImageValidationService
{
    /// <summary>
    /// Valida y procesa un archivo de imagen.
    /// Devuelve ImageValidationResult con bytes si es válido.
    /// </summary>
    Task<ImageValidationResult> ValidateAndProcessImageAsync(IFormFile? file);
}