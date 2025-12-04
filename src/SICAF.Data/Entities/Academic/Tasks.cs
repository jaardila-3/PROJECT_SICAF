using SICAF.Data.Entities.Common;

namespace SICAF.Data.Entities.Academic;

public class Tasks : BaseEntity
{
    public int Code { get; set; } // 1000, 2014, etc.
    public string Name { get; set; } = string.Empty;
    public string WingType { get; set; } = string.Empty;

    // Relaciones
    public virtual ICollection<StudentTaskGrade> StudentTaskGrades { get; set; } = [];
}