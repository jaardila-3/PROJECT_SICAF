namespace SICAF.Common.Interfaces;

// <summary>
/// Define los métodos para el hashing y verificación de contraseñas.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Genera un hash seguro de la contraseña proporcionada.
    /// </summary>
    /// <param name="password">La contraseña en texto plano.</param>
    /// <returns>El hash de la contraseña con el salt incluido.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifica si una contraseña coincide con su hash almacenado.
    /// </summary>
    /// <param name="hashedPassword">El hash almacenado de la contraseña.</param>
    /// <param name="providedPassword">La contraseña proporcionada por el usuario.</param>
    /// <returns>True si la contraseña es correcta; false en caso contrario.</returns>
    bool VerifyPassword(string hashedPassword, string providedPassword);

    /// <summary>
    /// Determina si un hash de contraseña necesita ser actualizado.
    /// </summary>
    /// <param name="hashedPassword">El hash actual de la contraseña.</param>
    /// <returns>True si el hash debe ser actualizado; false en caso contrario.</returns>
    bool NeedsRehash(string hashedPassword);
}