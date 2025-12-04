namespace SICAF.Web.Interfaces.Pdf;

/// <summary>
/// Servicio para generación de PDFs usando Puppeteer Sharp
/// </summary>
public interface IPuppeteerService
{
    /// <summary>
    /// Genera un PDF a partir de una URL
    /// </summary>
    /// <param name="url">URL de la página a convertir</param>
    /// <param name="waitForSelector">Selector CSS para esperar antes de generar el PDF</param>
    /// <param name="landscape">Si el PDF debe estar en orientación horizontal</param>
    /// <param name="cookies">Cookies de autenticación para pasar a Puppeteer</param>
    /// <returns>Bytes del PDF generado</returns>
    Task<byte[]> GeneratePdfFromUrlAsync(string url, bool landscape = false, IEnumerable<KeyValuePair<string, string>>? cookies = null);
}
