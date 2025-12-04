using SICAF.Data.Entities.Common;

namespace SICAF.Data.Entities.Identity;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsSystemRole { get; set; } = false;

    // relationships
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];

}