using Microsoft.EntityFrameworkCore.Storage;

using SICAF.Data.Entities.Common;

namespace SICAF.Data.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Obtiene un repositorio genérico para cualquier entidad
    /// </summary>
    IRepository<T> Repository<T>() where T : BaseEntity;

    /// <summary>
    /// Guarda los cambios
    /// </summary>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Manejo de transacciones
    /// </summary>
    Task<IDbContextTransaction> BeginTransactionAsync();
}