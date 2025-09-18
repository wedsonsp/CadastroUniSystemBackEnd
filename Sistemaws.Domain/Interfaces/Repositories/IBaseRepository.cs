using System.Collections.Generic;
using System.Threading.Tasks;
using Sistemaws.Domain.Entities;
using System.Linq.Expressions;
using System;

namespace Sistemaws.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
    where TId : notnull
{
    Task AddAsync(TEntity entity, int? userId = null);
    Task UpdateAsync(TEntity entity, int? userId = null);
    Task DeleteAsync(TEntity entity, int? userId = null);
    Task<TEntity?> GetByIdAsync(TId id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetPagedAsync(int page = 1, int pageSize = 10);
    Task<int> CountAsync();
    Task<bool> ExistsAsync(TId id);
}
