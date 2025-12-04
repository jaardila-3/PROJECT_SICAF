using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using SICAF.Data.Context;
using SICAF.Data.Entities.Common;
using SICAF.Data.Interfaces.Repositories;

namespace SICAF.Data.Repositories;

/// <summary>
/// Repositorio genérico
/// </summary>
/// <typeparam name="T">Entidad</typeparam>
/// <param name="context">Contexto</param>
public class Repository<T>(SicafDbContext context) : IRepository<T> where T : BaseEntity
{
    protected readonly SicafDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<IReadOnlyList<T>> GetListAsync()
        => await _dbSet.AsNoTracking().ToListAsync();

    public async Task<IReadOnlyList<T>> GetListAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).AsNoTracking().ToListAsync();

    public async Task<IReadOnlyList<T>> GetListAsync(
        Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false)
    {
        IQueryable<T> query = _dbSet;

        if (disableTracking)
            query = query.AsNoTracking();

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (includeFunc is not null)
            query = includeFunc(query);

        if (predicate is not null)
            query = query.Where(predicate);

        if (orderBy is not null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

    public async Task<T?> GetFirstAsync(
        Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false)
    {
        IQueryable<T> query = _dbSet;

        if (disableTracking)
            query = query.AsNoTracking();

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (includeFunc is not null)
            query = includeFunc(query);

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<T?> GetFirstAsync(
        Expression<Func<T, bool>>? predicate,
        List<string>? includeStrings,
        bool disableTracking = true,
        bool ignoreQueryFilters = false)
    {
        IQueryable<T> query = _dbSet;

        if (disableTracking)
            query = query.AsNoTracking();

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (includeStrings is not null)
        {
            foreach (var includeString in includeStrings)
            {
                if (!string.IsNullOrWhiteSpace(includeString))
                    query = query.Include(includeString);
            }
        }

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null)
    => predicate == null
        ? await _dbSet.AnyAsync()
        : await _dbSet.AnyAsync(predicate);

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        => predicate == null
            ? await _dbSet.CountAsync()
            : await _dbSet.CountAsync(predicate);


    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<T> entities)
        => await _dbSet.AddRangeAsync(entities);

    public void Update(T entity)
    {
        var entry = _context.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            _dbSet.Update(entity);
        }
        else
        {
            entry.State = EntityState.Modified;
        }
    }

    public void Delete(T entity)
        => _dbSet.Remove(entity);

    public void DeleteRange(IEnumerable<T> entities)
        => _dbSet.RemoveRange(entities);
}