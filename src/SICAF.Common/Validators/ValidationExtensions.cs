using System.Text.RegularExpressions;

using FluentValidation;

using SICAF.Common.Constants;

namespace SICAF.Common.Validators;

/// <summary>
/// Extensiones para las reglas de validación comunes en la aplicación
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// Aplica las reglas de validación para un nombre
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("El Nombre es obligatorio")
            .Length(3, 100).WithMessage("El Nombre debe tener entre 3 y 100 caracteres")
            .Matches(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ][a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ\s.]+$").WithMessage("El Nombre debe comenzar con una letra y solo puede contener letras, números, espacios y puntos");
    }

    /// <summary>
    /// Aplica las reglas de validación para un apellido
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateLastname<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(lastName => !string.IsNullOrWhiteSpace(lastName)).WithMessage("El Apellido es obligatorio")
            .Length(3, 50).WithMessage("El Apellido debe tener entre 3 y 50 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ][a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s.]+$").WithMessage("El Apellido debe comenzar con una letra y solo puede contener letras, espacios y puntos");
    }

    /// <summary>
    /// Aplica las reglas de validación para un nombre de usuario
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateUsername<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(username => !string.IsNullOrWhiteSpace(username)).WithMessage("El Usuario es obligatorio")
            .Length(8, 50).WithMessage("El Usuario debe tener entre 8 y 50 caracteres")
            //.Matches(@"^[a-z][a-z0-9]([._-]?[a-z0-9])*$").WithMessage("El Usuario debe comenzar con una letra. Puede contener letras o números, y un solo punto (.), guion (-) o guion bajo (_) entre caracteres")
            .Matches(@"^[a-z0-9]([._-]?[a-z0-9])*$").WithMessage("El Usuario debe contener letras o números, y un solo punto (.), guion (-) o guion bajo (_) entre caracteres")
            ;
    }

    /// <summary>
    /// Aplica las reglas de validación para una contraseña
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidatePassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("La Contraseña es obligatoria")
            .MinimumLength(8).WithMessage("La Contraseña debe tener al menos 8 caracteres")
            .MaximumLength(30).WithMessage("La Contraseña debe tener como máximo 30 caracteres")
            .Matches("[A-Z]").WithMessage("La Contraseña debe tener al menos una letra mayúscula.")
            .Matches("[a-z]").WithMessage("La Contraseña debe tener al menos una letra minúscula.")
            .Matches("[0-9]").WithMessage("La Contraseña debe tener al menos un dígito.")
            .Matches("[^a-zA-Z0-9]").WithMessage("La Contraseña debe tener al menos un carácter especial.");
    }

    /// <summary>
    /// Aplica las reglas de validación para una contraseña en Login, que puede ser el numero de identificación
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidatePasswordLogin<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("La Contraseña es obligatoria")
            .MinimumLength(8).WithMessage("La Contraseña debe tener al menos 8 caracteres")
            .MaximumLength(30).WithMessage("La Contraseña debe tener como máximo 30 caracteres");
    }

    /// <summary>
    /// Aplica las reglas de validación para un motivo (texto)
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidateDescription<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Length(0, 255).WithMessage("Este campo no puede exceder los 255 caracteres")
            .Matches(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚüÜñÑ\s.,;:()\-_!¡?¿""']+$")
                .WithMessage("El texto contiene caracteres no permitidos. Solo se permiten letras, números y signos básicos de puntuación.")
            .Must(s => !ContainsHtmlTags(s)).WithMessage("El texto no debe contener etiquetas HTML o scripts.")
            .Must(s => !ContainsJavaScriptEvents(s)).WithMessage("El texto contiene contenido potencialmente inseguro.");
    }

    /// <summary>
    /// Valida el número de identificación
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateIdentificationNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(number => !string.IsNullOrWhiteSpace(number)).WithMessage("El número de identificación es obligatorio")
            .Length(8, 25).WithMessage("El número de identificación debe tener entre 8 y 25 dígitos")
            .Matches(@"^[a-zA-Z0-9]+$").WithMessage("El número de identificación solo debe contener números y letras");
    }

    /// <summary>
    /// Valida la nacionalidad
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateNationality<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(nationality => !string.IsNullOrWhiteSpace(nationality)).WithMessage("La nacionalidad es obligatoria")
            .Length(3, 50).WithMessage("La nacionalidad debe tener entre 3 y 50 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ][a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$").WithMessage("La nacionalidad solo debe contener letras y espacios");
    }

    /// <summary>
    /// Valida el tipo de sangre
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateBloodType<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(bloodType => !string.IsNullOrWhiteSpace(bloodType)).WithMessage("El tipo de sangre es obligatorio")
            .Must(bloodType => ValidTypes.Contains(bloodType?.ToUpper()))
                .WithMessage($"El tipo de sangre no es válido. Tipos válidos: {string.Join(", ", ValidTypes)}");
    }

    /// <summary>
    /// Valida la fecha de nacimiento
    /// </summary>
    public static IRuleBuilderOptions<T, DateTime> ValidateBirthDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("La fecha de nacimiento es obligatoria")
            .Must(birthDate => birthDate <= DateTime.Today.AddYears(-16)).WithMessage("La persona debe tener al menos 16 años")
            .Must(birthDate => birthDate >= DateTime.Today.AddYears(-80)).WithMessage("La fecha de nacimiento no puede ser anterior a 80 años");
    }

    /// <summary>
    /// Valida el número de teléfono colombiano
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidatePhoneNumber<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Length(0, 20).WithMessage("El número de teléfono no puede exceder 20 caracteres");
    }

    #region Validaciones de seguridad
    /// <summary>
    /// Verifica si el texto contiene etiquetas HTML
    /// </summary>
    private static bool ContainsHtmlTags(string text)
    {
        if (string.IsNullOrEmpty(text))
            return false;

        // Detecta etiquetas HTML comunes
        return Regex.IsMatch(text, @"<[^>]*>");
    }

    /// <summary>
    /// Verifica si el texto contiene eventos JavaScript potencialmente maliciosos
    /// </summary>
    private static bool ContainsJavaScriptEvents(string text)
    {
        if (string.IsNullOrEmpty(text))
            return false;

        // Detecta patrones comunes de eventos JavaScript y URLs maliciosas
        string[] patrones =
        [
            @"javascript:",
            @"onload\s*=",
            @"onerror\s*=",
            @"onclick\s*=",
            @"onmouseover\s*=",
            @"onmouseout\s*=",
            @"onkeypress\s*=",
            @"onkeydown\s*=",
            @"onkeyup\s*=",
            @"onsubmit\s*=",
            @"onblur\s*=",
            @"onfocus\s*=",
            @"alert\s*\(",
            @"eval\s*\(",
            @"document\.cookie",
            @"document\.write",
            @"\.href",
            @"data:text/html"
        ];

        foreach (var patron in patrones)
        {
            if (Regex.IsMatch(text, patron, RegexOptions.IgnoreCase))
                return true;
        }

        return false;
    }

    public static readonly string[] ValidTypes = ["A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-"];

    #endregion
}