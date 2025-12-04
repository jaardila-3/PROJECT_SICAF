using System.Collections;

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

using SICAF.Data.Context;
using SICAF.Data.Entities.Common;
using SICAF.Data.Interfaces.Repositories;

namespace SICAF.Data.Repositories;

/// <summary>
/// Unidad de trabajo
/// </summary>
/// <param name="context">Contexto</param>
/// <param name="logger">Logger</param>
public class UnitOfWork(
    SicafDbContext context,
    ILogger<UnitOfWork> logger) : IUnitOfWork
{
    private readonly Hashtable _repositories = [];
    private readonly SicafDbContext _context = context;
    private readonly ILogger<UnitOfWork> _logger = logger;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    /// <summary>
    /// Obtiene un repositorio genérico para cualquier entidad
    /// </summary>
    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);
        var typeName = type.Name;

        if (!_repositories.ContainsKey(typeName))
        {
            var repositoryType = typeof(Repository<>);
            var repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(type), _context);

            _repositories[typeName] = repositoryInstance;
        }

        return (IRepository<T>)_repositories[typeName]!;
    }

    /// <summary>
    /// Guarda los cambios
    /// </summary>
    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar cambios en UnitOfWork");
            throw new Exception($"Error al guardar cambios: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Inicia una transacción
    /// </summary>
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        try
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al iniciar transacción");
            throw;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}