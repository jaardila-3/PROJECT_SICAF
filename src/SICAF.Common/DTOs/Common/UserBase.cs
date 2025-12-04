using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Common;

public abstract class UserBase
{
    [Display(Name = "Tipo de Documento")]
    [Required(ErrorMessage = "El tipo de documento es obligatorio")]
    public string DocumentType { get; set; } = string.Empty;

    [Display(Name = "N° Documento")]
    [Required(ErrorMessage = "El número de identificación es obligatorio")]
    public string IdentificationNumber { get; set; } = string.Empty;

    [Display(Name = "Grado")]
    public string? Grade { get; set; }

    [Display(Name = "Antigüedad en el Grado")]
    public int? SeniorityOrder { get; set; }

    [Display(Name = "Correo Electrónico")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    public string? Email { get; set; }

    [Display(Name = "Nombres")]
    [Required(ErrorMessage = "Los nombres son obligatorios")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Apellidos")]
    [Required(ErrorMessage = "Los apellidos son obligatorios")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Nacionalidad")]
    [Required(ErrorMessage = "La nacionalidad es obligatoria")]
    public string Nationality { get; set; } = string.Empty;

    [Display(Name = "Tipo de Sangre")]
    [Required(ErrorMessage = "El tipo de sangre es obligatorio")]
    public string BloodType { get; set; } = string.Empty;

    [Display(Name = "Fecha de Nacimiento")]
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    public string BirthDateString { get; set; } = DateTime.Today.AddYears(-25).ToString("yyyy-MM-dd");

    public DateTime BirthDate { get; set; }

    [Display(Name = "Fuerza")]
    public string? Force { get; set; }

    // Foto
    public string? PhotoFileName { get; set; }
    public byte[]? PhotoData { get; set; }
    public string? PhotoContentType { get; set; }

    [Display(Name = "Usuario")]
    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    public string Username { get; set; } = string.Empty;

    [Display(Name = "Número de Teléfono")]
    [Phone(ErrorMessage = "El formato del número de teléfono no es válido")]
    [DataType(DataType.Text)]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Fecha de Desbloqueo")]
    [DataType(DataType.Text)]
    public DateTime? LockoutEnd { get; set; }

    [Display(Name = "Motivo de Bloqueo")]
    public string? LockoutReason { get; set; }

    /// <summary>
    /// Indica si el usuario está actualmente bloqueado
    /// </summary>
    [Display(Name = "Estado")]
    public bool IsLockedOut => LockoutEnd.HasValue && LockoutEnd.Value > DateTime.Now;

    /// <summary>
    /// Nombre completo del usuario
    /// </summary>
    [Display(Name = "Nombre Completo")]
    public string FullName => Grade != null ? $"{Grade}. {Name} {LastName}".Trim() : $"{Name} {LastName}".Trim();

    /// <summary>
    /// Identificación completa (Tipo + Número)
    /// </summary>
    [Display(Name = "Identificación")]
    public string FullIdentification => $"{DocumentType}: {IdentificationNumber}";

    /// <summary>
    /// Edad del usuario basada en la fecha de nacimiento
    /// </summary>
    [Display(Name = "Edad")]
    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

    /// <summary>
    /// Base 64 de la fotografia
    /// </summary>
    public string? PhotoBase64 => PhotoData != null && PhotoData.Length > 0
    ? $"data:{PhotoContentType};base64,{Convert.ToBase64String(PhotoData)}"
    : null;

    /// <summary>
    /// Orden jerárquico del grado militar para ordenamiento
    /// </summary>
    [Display(Name = "Orden de Grado")]
    public int GradeOrder { get; set; } = 999;
}