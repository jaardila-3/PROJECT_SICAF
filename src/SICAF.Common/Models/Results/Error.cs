namespace SICAF.Common.Models.Results;

/// <summary>
/// Representa un error con código y mensaje
/// </summary>
public class Error(string code, string message)
{
    public string Code { get; } = code;
    public string Message { get; } = message;

    // Error vacío para resultados exitosos
    public static Error None => new(string.Empty, string.Empty);

    // Método helper para crear errores rápidamente
    public static Error New(string code, string message) => new(code, message);
}