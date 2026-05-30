using System.ComponentModel.Design;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

using WMSCommon.Contexts;
using WMSCommon.Contracts;
using WMSCommon.Entities;
using WMSCommon.Results;

namespace WMSCommon.Repositories
{
    public abstract class TenantRepository<T, TDbContext>
        : ITenantRepository<T>
        where T : class, ITenantEntity
        where TDbContext : DbContext
    {
        protected TDbContext DbContext { get; }
        protected IUserContext UserContext { get; }

        protected TenantRepository(TDbContext dbContext,
            IUserContext userContext)
        {
            DbContext = dbContext;
            UserContext = userContext;
        }
        
        public async Task<T?> GetByIdAsync(
            Guid id, 
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = DbContext.Set<T>().AsNoTracking();
            if (include != null)
            {
                query = include(query);
            }

            return await query
                .Where(e => e.CompanyId == UserContext.CompanyId)
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

            Guid companyId = UserContext.CompanyId;
            IQueryable<T> query = DbContext.Set<T>()
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
            if (entity is ISyncEntity syncEntity)
            {
                syncEntity.Version = 1;
            }
            
            entity.CompanyId = UserContext.CompanyId;
            await DbContext.Set<T>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            DbContext.Entry(entity).State = EntityState.Detached;
            return RepositoryResult<T>.Success(entity);
        }

        public async Task<RepositoryResult<T>> DeleteAsync(Guid id)
        {
            var entity = await DbContext.Set<T>()
                .FirstOrDefaultAsync(e => e.Id == id && e.CompanyId == UserContext.CompanyId);
            if (entity == null)
            {
                return RepositoryResult<T>.Failure($"{typeof(T).Name} not found.");
            }

            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync();
            return RepositoryResult<T>.Success(entity);
        }

        public async Task<int> CountAsync()
        {
            Guid companyId = UserContext.CompanyId;
            return await DbContext.Set<T>()
                .Where(e => e.CompanyId == companyId)
                .CountAsync();
        }

        public async Task<RepositoryResult<T>> UpdateAsync(T entity)
        {
            var existing = await DbContext.Set<T>().FindAsync(entity.Id);
            if (existing == null)
            {
                return RepositoryResult<T>.Failure($"{typeof(T).Name} not found.");
            }
            // if existing entity and UserContext company Id different, dont allow editing
            if (existing.CompanyId != UserContext.CompanyId)
            {
                return RepositoryResult<T>.Failure($"Cannot edit {typeof(T).Name} from different company");
            }
            
            if (entity is ISyncEntity syncEntity)
            {
                syncEntity.Version++;
            }
            
            entity.CompanyId = UserContext.CompanyId;
            DbContext.Entry(existing).CurrentValues.SetValues(entity);

            await DbContext.SaveChangesAsync();
            return RepositoryResult<T>.Success(existing);
        }
        
        
        protected async Task AssignEntitiesAsync<TEntity>(
            IEnumerable<Guid> ids,
            DbSet<TEntity> dbSet,
            ICollection<TEntity> targetCollection)
            where TEntity : class, IGenericEntity
        {
            var idList = ids.ToList();
            if (!idList.Any())
            {
                return;
            }

            var entities = await dbSet
                .Where(e => idList.Contains(e.Id))
                .ToListAsync();

            foreach (var entity in entities)
            {
                targetCollection.Add(entity);
            }
        }
        
        protected async Task<RepositoryResult<TEntity>> GetAndValidateAsync<TEntity>(
            Guid id,
            IQueryable<TEntity> query)
            where TEntity : class, ITenantEntity
        {
            var entity = await query.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return RepositoryResult<TEntity>.Failure($"{typeof(TEntity).Name} not found.");
            }

            if (entity.CompanyId != UserContext.CompanyId)
            {
                return RepositoryResult<TEntity>.Failure($"Cannot edit {typeof(TEntity).Name} from different company");
            }

            return RepositoryResult<TEntity>.Success(entity);
        }
        
        protected static async Task AddIdsToCollectionAsync<TEntity>(
            ICollection<TEntity> existingCollection,
            IEnumerable<Guid> incomingIds,
            IQueryable<TEntity> dbSet)
            where TEntity : class, IGenericEntity
        {
            var incomingSet = incomingIds.ToHashSet();

            // Remove entities not in incoming IDs
            var toRemove = existingCollection
                .Where(e => !incomingSet.Contains(e.Id))
                .ToList();

            foreach (var item in toRemove)
            {
                existingCollection.Remove(item);
            }

            // Get current IDs after removal
            var existingIds = existingCollection
                .Select(e => e.Id)
                .ToHashSet();

            // Determine IDs to add
            var idsToAdd = incomingSet
                .Where(id => !existingIds.Contains(id))
                .ToList();

            if (idsToAdd.Count > 0)
            {
                var entitiesToAdd = await dbSet
                    .Where(e => idsToAdd.Contains(e.Id))
                    .ToListAsync();

                foreach (var entity in entitiesToAdd)
                {
                    existingCollection.Add(entity);
                }
            }
        }
    }
}
