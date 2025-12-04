using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.Configuration.Options;

public class ImageValidationOptions
{
    public const string SectionName = "ImageValidationSettings";

    [Required]
    public string[] AllowedExtensions { get; set; } = [".png"];

    [Required]
    public string[] AllowedMimeTypes { get; set; } = ["image/png"];

    public long MaxFileSizeBytes { get; set; } = 2 * 1024 * 1024; // 2 MB
}