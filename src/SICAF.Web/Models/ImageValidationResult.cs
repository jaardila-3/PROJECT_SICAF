namespace SICAF.Web.Models;

public class ImageValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public byte[]? ImageData { get; set; }
    public string? ContentType { get; set; }
    public string? FileName { get; set; }
}