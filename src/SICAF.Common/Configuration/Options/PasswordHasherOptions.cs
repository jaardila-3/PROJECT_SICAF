using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace SICAF.Common.Configuration.Options;

/// <summary>
/// Contiene las opciones de configuración para el servicio de hashing de contraseñas.
/// </summary>
public class PasswordHasherOptions
{
    /// <summary>
    /// El nombre de la sección en los archivos de configuración.
    /// </summary>
    public const string SectionName = "PasswordHasherSettings";

    /// <summary>
    /// El número de iteraciones para el algoritmo PBKDF2.
    /// Un número más alto aumenta la seguridad pero también el tiempo de procesamiento.
    /// </summary>
    [Range(100000, 500000, ErrorMessage = "El número de iteraciones debe estar entre 100,000 y 500,000.")]
    public int Iterations { get; set; } = 150000;

    /// <summary>
    /// El tamaño del salt en bytes. 16 bytes (128 bits) es un valor seguro.
    /// </summary>
    [Range(16, 64, ErrorMessage = "El tamaño del salt debe estar entre 16 y 64 bytes.")]
    public int SaltSize { get; set; } = 16;

    /// <summary>
    /// El tamaño del hash final en bytes. 32 bytes (256 bits) para SHA256.
    /// </summary>
    [Range(32, 128, ErrorMessage = "El tamaño del hash debe estar entre 32 y 128 bytes.")]
    public int HashSize { get; set; } = 32;

    /// <summary>
    /// El nombre del algoritmo de hash a utilizar (ej. "SHA256", "SHA512").
    /// </summary>
    [Required]
    public string AlgorithmName { get; set; } = "SHA256";

    /// <summary>
    /// Propiedad computada que convierte el nombre del algoritmo en el tipo HashAlgorithmName requerido.
    /// No se mapea desde la configuración, sino que se deriva de AlgorithmName.
    /// </summary>
    public HashAlgorithmName Algorithm => new(AlgorithmName);
}