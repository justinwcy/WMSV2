using System.Linq.Expressions;

using WMSCommon.Entities;
using WMSCommon.Results;

namespace WMSCommon.Repositories
{
    public interface IGenericRepository<T> where T : IGenericEntity
    {
        public Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null);

        public Task<IReadOnlyList<T>> GetAsync(
            int pageNumber, 
            int pageSize,
            Expression<Func<T, object>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            bool descending = false);

        public Task<RepositoryResult<T>> CreateAsync(T entity);

        public Task<RepositoryResult<T>> UpdateAsync(T entity);

        public Task<RepositoryResult<T>> DeleteAsync(Guid id);

        // count total records for pagination metadata
        public Task<int> CountAsync();
    }
}
