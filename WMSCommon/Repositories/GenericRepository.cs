using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using WMSCommon.Entities;
using WMSCommon.Results;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WMSCommon.Repositories
{
    public abstract class GenericRepository<T, TContext>(
        IDbContextFactory<TContext> dbContextFactory)
        : IGenericRepository<T>
        where T : class, IGenericEntity
        where TContext : DbContext
    {
        protected readonly IDbContextFactory<TContext> ContextFactory = dbContextFactory;

        public async Task<T?> GetByIdAsync(
            Guid id, 
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            IQueryable<T> query = dbContext.Set<T>().AsNoTracking();
            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IReadOnlyList<T>> GetAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, object>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            bool descending = false)
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            int skip = (Math.Max(1, pageNumber) - 1) * pageSize;

            IQueryable<T> query = dbContext.Set<T>().AsNoTracking();
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
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            dbContext.Entry(entity).State = EntityState.Detached;
            return RepositoryResult<T>.Success(entity);
        }

        public async Task<RepositoryResult<T>> DeleteAsync(Guid id)
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var entity = await dbContext.Set<T>()
                .FirstOrDefaultAsync(e => e.Id == id);
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
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            return await dbContext.Set<T>().CountAsync();
        }

        public async Task<RepositoryResult<T>> UpdateAsync(T entity)
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
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
