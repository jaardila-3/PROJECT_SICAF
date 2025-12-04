using Microsoft.Extensions.Logging;

using SICAF.Business.Interfaces.Catalogs;
using SICAF.Common.Models.Results;
using SICAF.Data.Entities.Catalogs;
using SICAF.Data.Interfaces.Repositories;

namespace SICAF.Business.Services.Catalogs;

public class CatalogService(IUnitOfWork unitOfWork, ILogger<CatalogService> logger) : ICatalogService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CatalogService> _logger = logger;

    public async Task<Result<IEnumerable<MasterCatalog>>> GetByCatalogTypeAsync(string catalogType)
    {
        var entities = await _unitOfWork.Repository<MasterCatalog>()
                .GetListAsync(
                    predicate: c => c.CatalogType == catalogType,
                    orderBy: q => q.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name)
                );
        return entities == null
            ? Result<IEnumerable<MasterCatalog>>.Failure("CATALOG_NOT_FOUND", "No se encontraron datos.")
            : Result<IEnumerable<MasterCatalog>>.Success(entities);
    }

    /// <summary>
    /// Obtiene catálogos de múltiples tipos en una sola consulta
    /// </summary>
    /// <param name="catalogTypes">Lista de tipos de catálogo a obtener</param>
    /// <returns>Diccionario con los catálogos agrupados por tipo</returns>
    public async Task<Result<Dictionary<string, List<MasterCatalog>>>> GetByCatalogTypesAsync(params string[] catalogTypes)
    {
        try
        {
            // Validación de entrada
            if (catalogTypes == null || catalogTypes.Length == 0)
            {
                return Result<Dictionary<string, List<MasterCatalog>>>.Failure("CATALOGTYPES_NULL", "Debe especificar al menos un tipo de catálogo");
            }

            // Consulta única para obtener todos los tipos solicitados
            var entities = await _unitOfWork.Repository<MasterCatalog>()
                .GetListAsync(
                    predicate: c => catalogTypes.Contains(c.CatalogType),
                    orderBy: q => q.OrderBy(x => x.CatalogType)
                                  .ThenBy(x => x.DisplayOrder)
                                  .ThenBy(x => x.Name)
                );

            // Agrupar por tipo de catálogo
            var groupedCatalogs = entities
                .GroupBy(c => c.CatalogType)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToList()
                );

            // Verificar que se obtuvieron todos los tipos solicitados
            foreach (var requestedType in catalogTypes)
            {
                if (!groupedCatalogs.ContainsKey(requestedType))
                {
                    // Agregar lista vacía para tipos sin datos
                    groupedCatalogs[requestedType] = [];
                }
            }

            return Result<Dictionary<string, List<MasterCatalog>>>.Success(groupedCatalogs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener múltiples tipos de catálogo");
            return Result<Dictionary<string, List<MasterCatalog>>>.Failure("CATALOGS_ERROR", "Error al obtener listado");
        }
    }
}