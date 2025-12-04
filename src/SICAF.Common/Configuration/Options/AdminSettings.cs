using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.Configuration.Options;

public class AdminSettings
{
    public const string SectionName = "AdminSettings";

    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string IdentificationNumber { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string Username { get; set; } = string.Empty;

    public string DocumentType { get; set; } = "CC";

    public string? Grade { get; set; }

    public string Nationality { get; set; } = "Colombiana";

    public string BloodType { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public string? Force { get; set; }
}