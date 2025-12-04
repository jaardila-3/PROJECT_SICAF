using PuppeteerSharp;
using PuppeteerSharp.Media;

using SICAF.Web.Interfaces.Pdf;

namespace SICAF.Web.Services.Pdf;

/// <summary>
/// Servicio para generación de PDFs usando Puppeteer Sharp
/// </summary>
public class PuppeteerService() : IPuppeteerService
{
    /// <summary>
    /// Genera un PDF a partir de una URL
    /// </summary>
    /// <param name="url">URL de la página a convertir</param>
    /// <param name="landscape">Si el PDF debe estar en orientación horizontal</param>
    /// <param name="cookies">Cookies de autenticación para pasar a Puppeteer</param>
    /// <returns>Bytes del PDF generado</returns>
    public async Task<byte[]> GeneratePdfFromUrlAsync(string url, bool landscape = false, IEnumerable<KeyValuePair<string, string>>? cookies = null)
    {
        // Descargar Chromium si no existe (solo se hace una vez, ~150MB)
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync(); // Descarga la versión por defecto

        // Configurar opciones del navegador headless (sin interfaz gráfica)
        var launchOptions = new LaunchOptions
        {
            Headless = true,
            Args =
            [
                "--no-sandbox",                 // Necesario para Azure/Docker
                    "--disable-setuid-sandbox",     // Necesario para Azure/Docker
                    "--disable-dev-shm-usage",      // Evita problemas de memoria en contenedores
                    "--disable-gpu"                 // No necesitamos GPU en servidor
            ]
        };

        await using var browser = await Puppeteer.LaunchAsync(launchOptions);
        await using var page = await browser.NewPageAsync();

        // Viewport grande para mejor calidad de renderizado
        await page.SetViewportAsync(new ViewPortOptions
        {
            Width = 1920,
            Height = 1080
        });

        // Si se proporcionan cookies, configurarlas antes de navegar
        if (cookies != null && cookies.Any())
        {
            var uri = new Uri(url);

            // Extraer solo el hostname sin puerto
            var domain = uri.Host;
            if (domain.Contains(':'))
            {
                domain = domain.Split(':')[0];
            }

            var cookieParams = cookies.Select(c => new CookieParam
            {
                Name = c.Key,
                Value = c.Value,
                Domain = domain,
                Path = "/",
                HttpOnly = true,
                Secure = uri.Scheme == "https",
                SameSite = SameSite.Lax
            }).ToArray();

            await page.SetCookieAsync(cookieParams);
        }

        await page.GoToAsync(url, new NavigationOptions
        {
            // Networkidle0: espera hasta que no haya conexiones de red activas
            // Esto asegura que JS, CSS, imágenes y gráficos hayan cargado completamente
            WaitUntil = [WaitUntilNavigation.Networkidle0],
            Timeout = 60000 // Timeout máximo 60 segundos
        });

        var pdfOptions = new PdfOptions
        {
            Format = PaperFormat.A4,
            PrintBackground = false, // Incluir colores de fondo y CSS backgrounds
            Landscape = landscape,
            MarginOptions = new MarginOptions
            {
                Top = "10mm",
                Right = "10mm",
                Bottom = "10mm",
                Left = "10mm"
            },
            PreferCSSPageSize = false
        };

        var pdfBytes = await page.PdfDataAsync(pdfOptions);

        return pdfBytes;
    }
}
