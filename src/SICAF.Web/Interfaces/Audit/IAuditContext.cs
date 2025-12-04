namespace SICAF.Web.Interfaces.Audit;

/// <summary>
/// Contexto de información para auditoría
/// </summary>
public interface IAuditContext
{
    Guid LoggedUserId { get; }
    string UserName { get; }
    string IdentificationNumber { get; }
    string Grade { get; }
    string WingType { get; }
    string Name { get; }
    string LastName { get; }
    string? UserRole { get; }
    string? IpAddress { get; }
    string? HttpMethod { get; }
    string? Url { get; }
    string? Module { get; }
    string? Controller { get; }
    string? Action { get; }
}