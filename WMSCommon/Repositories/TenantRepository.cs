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
        protected readonly IDbContextFactory<TContext> ContextFactory = dbContextFactory;
        protected readonly IUserContext UserContext = userContext;

        public async Task<T?> GetByIdAsync(Guid id)
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            return await dbContext.Set<T>()
                .AsNoTracking()
                .Where(e => e.CompanyId == userContext.CompanyId)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IReadOnlyList<T>> GetAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, object>>? orderBy = null,
            bool descending = false)
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            int skip = (Math.Max(1, pageNumber) - 1) * pageSize;

            Guid companyId = UserContext.CompanyId;
            IQueryable<T> query = dbContext.Set<T>()
                .AsNoTracking()
                .Where(e=> e.CompanyId == companyId);

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
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            dbContext.Entry(entity).State = EntityState.Detached;
            return RepositoryResult<T>.Success(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync();

            Guid companyId = UserContext.CompanyId;
            int rowsAffected = await dbContext.Set<T>()
                .Where(e => e.Id == id && e.CompanyId == companyId)
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }

        public async Task<int> CountAsync()
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            Guid companyId = UserContext.CompanyId;
            return await dbContext.Set<T>()
                .Where(e => e.CompanyId == companyId)
                .CountAsync();
        }

        public abstract Task<RepositoryResult<T>> UpdateAsync(T entity);
    }
}
