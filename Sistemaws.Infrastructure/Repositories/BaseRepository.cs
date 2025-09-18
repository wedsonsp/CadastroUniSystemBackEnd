using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sistemaws.Domain.Entities;
using Sistemaws.Domain.Interfaces.Repositories;
using Sistemaws.Infrastructure.Persistence;

namespace Sistemaws.Infrastructure.Repositories;

public class BaseRepository<TEntity, TId>(SistemawsDbContext context) : IBaseRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
    where TId : notnull
{
    protected readonly SistemawsDbContext _context = context;

    public virtual async Task AddAsync(TEntity entity, int? userId = null)
    {
        if (userId.HasValue)
        {
            entity.CreatedBy = userId;
        }
        entity.CreatedAt = DateTime.UtcNow;
        entity.Deleted = false;
        
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity, int? userId = null)
    {
        if (userId.HasValue)
        {
            entity.UpdatedBy = userId;
        }
        entity.UpdatedAt = DateTime.UtcNow;
        
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity, int? userId = null)
    {
        if (userId.HasValue)
        {
            entity.UpdatedBy = userId;
        }
        entity.UpdatedAt = DateTime.UtcNow;
        entity.Deleted = true;
        
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await _context.Set<TEntity>()
            .FirstOrDefaultAsync(e => e.Id.Equals(id) && !e.Deleted);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>()
            .Where(e => !e.Deleted)
            .ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetPagedAsync(int page = 1, int pageSize = 10)
    {
        return await _context.Set<TEntity>()
            .Where(e => !e.Deleted)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public virtual async Task<int> CountAsync()
    {
        return await _context.Set<TEntity>()
            .Where(e => !e.Deleted)
            .CountAsync();
    }

    public virtual async Task<bool> ExistsAsync(TId id)
    {
        return await _context.Set<TEntity>()
            .AnyAsync(e => e.Id.Equals(id) && !e.Deleted);
    }
}
