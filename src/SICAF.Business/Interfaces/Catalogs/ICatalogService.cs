using SICAF.Common.Models.Results;
using SICAF.Data.Entities.Catalogs;

namespace SICAF.Business.Interfaces.Catalogs;

public interface ICatalogService
{
    Task<Result<IEnumerable<MasterCatalog>>> GetByCatalogTypeAsync(string catalogType);
    Task<Result<Dictionary<string, List<MasterCatalog>>>> GetByCatalogTypesAsync(params string[] catalogTypes);
}