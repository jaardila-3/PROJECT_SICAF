using SICAF.Data.Entities.Common;

namespace SICAF.Data.Entities.Identity;

public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime? ExpiresAt { get; set; } // Para roles temporales

    // relationships
    public virtual User User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;

}