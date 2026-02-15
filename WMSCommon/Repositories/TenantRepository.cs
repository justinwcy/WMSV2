using System.ComponentModel.Design;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using WMSCommon.Contexts;
using WMSCommon.Entities;
using WMSCommon.Results;

namespace WMSCommon.Repositories
{
    public abstract class TenantRepository<T, TContext>(
        IDbContextFactory<TContext> dbContextFactory,
        IUserContext userContext)
        : ITenantRepository<T>
        where T : class, ITenantEntity
        where TContext : DbContext
    {
        public async Task<T?> GetByIdAsync(
            Guid id, 
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
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
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
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
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            dbContext.Entry(entity).State = EntityState.Detached;
            return RepositoryResult<T>.Success(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            Guid companyId = userContext.CompanyId;
            int rowsAffected = await dbContext.Set<T>()
                .Where(e => e.Id == id && e.CompanyId == companyId)
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }

        public async Task<int> CountAsync()
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            Guid companyId = userContext.CompanyId;
            return await dbContext.Set<T>()
                .Where(e => e.CompanyId == companyId)
                .CountAsync();
        }

        public abstract Task<RepositoryResult<T>> UpdateAsync(T entity);
    }
}
