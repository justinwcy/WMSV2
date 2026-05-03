using System.ComponentModel.Design;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

using WMSCommon.Contexts;
using WMSCommon.Entities;
using WMSCommon.Results;

namespace WMSCommon.Repositories
{
    public abstract class TenantRepository<T, TDbContext>(
        TDbContext dbContext,
        IUserContext userContext)
        : ITenantRepository<T>
        where T : class, ITenantEntity
        where TDbContext : DbContext
    {
        public async Task<T?> GetByIdAsync(
            Guid id, 
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = dbContext.Set<T>().AsNoTracking();
            if (include != null)
            {
                query = include(query);
            }

            return await query
                .Where(e => e.CompanyId == userContext.CompanyId)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IReadOnlyList<T>> GetAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, object>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            bool descending = false)
        {
            int skip = (Math.Max(1, pageNumber) - 1) * pageSize;

            Guid companyId = userContext.CompanyId;
            IQueryable<T> query = dbContext.Set<T>()
                .AsNoTracking()
                .Where(e=> e.CompanyId == companyId);

            if (include != null)
            {
                query = include(query);
            }

            // Handle Dynamic Ordering
            if (orderBy != null)
            {
                query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }
            else
            {
                // order by ID to ensure consistent paging if no order is provided
                query = query.OrderBy(e => e.Id);
            }

            return await query.Skip(skip).Take(pageSize).ToListAsync();
        }

        public async Task<RepositoryResult<T>> CreateAsync(T entity)
        {
            entity.CompanyId = userContext.CompanyId;
            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            dbContext.Entry(entity).State = EntityState.Detached;
            return RepositoryResult<T>.Success(entity);
        }

        public async Task<RepositoryResult<T>> DeleteAsync(Guid id)
        {
            var entity = await dbContext.Set<T>()
                .FirstOrDefaultAsync(e => e.Id == id && e.CompanyId == userContext.CompanyId);
            if (entity == null)
            {
                return RepositoryResult<T>.Failure("Entity not found.");
            }

            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
            return RepositoryResult<T>.Success(entity);
        }

        public async Task<int> CountAsync()
        {
            Guid companyId = userContext.CompanyId;
            return await dbContext.Set<T>()
                .Where(e => e.CompanyId == companyId)
                .CountAsync();
        }

        public async Task<RepositoryResult<T>> UpdateAsync(T entity)
        {
            var existing = await dbContext.Set<T>().FindAsync(entity.Id);
            if (existing == null)
            {
                return RepositoryResult<T>.Failure($"{typeof(T).Name} not found.");
            }

            dbContext.Entry(existing).CurrentValues.SetValues(entity);

            await dbContext.SaveChangesAsync();
            return RepositoryResult<T>.Success(existing);
        }
    }
}
