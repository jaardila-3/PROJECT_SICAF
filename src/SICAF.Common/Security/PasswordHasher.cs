using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Options;

using SICAF.Common.Configuration.Options;
using SICAF.Common.Interfaces;

namespace SICAF.Common.Security;

/// <summary>
/// Implementación segura de hashing de contraseñas usando PBKDF2 con SHA512.
/// </summary>
public sealed class PasswordHasher(IOptions<PasswordHasherOptions> options) : IPasswordHasher
{
    private readonly PasswordHasherOptions _options = options?.Value ?? new PasswordHasherOptions();
    private const string Delimiter = ".";
    private const int CurrentVersion = 1;

    public string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));

        // Generar salt aleatorio
        byte[] salt = RandomNumberGenerator.GetBytes(_options.SaltSize);

        // Generar hash
        byte[] hash = GenerateHash(password, salt, _options.Iterations);

        // Formato: version.iterations.salt.hash
        return $"{CurrentVersion}{Delimiter}" +
               $"{_options.Iterations}{Delimiter}" +
               $"{Convert.ToBase64String(salt)}{Delimiter}" +
               $"{Convert.ToBase64String(hash)}";
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(providedPassword))
            return false;

        try
        {
            var parts = hashedPassword.Split(Delimiter);
            if (parts.Length != 4)
                return false;

            // Extraer componentes
            if (!int.TryParse(parts[0], out int version) || version != CurrentVersion)
                return false;

            if (!int.TryParse(parts[1], out int iterations))
                return false;

            byte[] salt = Convert.FromBase64String(parts[2]);
            byte[] storedHash = Convert.FromBase64String(parts[3]);

            // Verificar longitudes
            if (salt.Length != _options.SaltSize || storedHash.Length != _options.HashSize)
                return false;

            // Generar hash con la contraseña proporcionada
            byte[] providedHash = GenerateHash(providedPassword, salt, iterations);

            // Comparación segura contra ataques de timing
            return CryptographicOperations.FixedTimeEquals(providedHash, storedHash);
        }
        catch (FormatException)
        {
            // El formato del hash almacenado es inválido
            return false;
        }
    }

    public bool NeedsRehash(string hashedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword))
            return true;

        try
        {
            var parts = hashedPassword.Split(Delimiter);
            if (parts.Length != 4)
                return true;

            // Verificar versión
            if (!int.TryParse(parts[0], out int version) || version != CurrentVersion)
                return true;

            // Verificar si las iteraciones han cambiado
            if (!int.TryParse(parts[1], out int iterations) || iterations != _options.Iterations)
                return true;

            // Verificar tamaños
            byte[] salt = Convert.FromBase64String(parts[2]);
            byte[] hash = Convert.FromBase64String(parts[3]);

            return salt.Length != _options.SaltSize || hash.Length != _options.HashSize;
        }
        catch (FormatException)
        {
            return true;
        }
    }

    private byte[] GenerateHash(string password, byte[] salt, int iterations)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
        password: password,
        salt: salt,
        iterations: iterations,
        hashAlgorithm: _options.Algorithm,
        outputLength: _options.HashSize);
    }
}
