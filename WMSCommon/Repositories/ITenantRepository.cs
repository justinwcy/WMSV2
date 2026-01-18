using System.Linq.Expressions;

using WMSCommon.Entities;
using WMSCommon.Results;

namespace WMSCommon.Repositories
{
    public interface ITenantRepository<T> where T : ITenantEntity
    {
        Task<T?> GetByIdAsync(Guid id);

        Task<IReadOnlyList<T>> GetAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, object>>? orderBy = null,
            bool descending = false);

        Task<RepositoryResult<T>> CreateAsync(T entity);

        public Task<RepositoryResult<T>> UpdateAsync(T entity);

        public Task<bool> DeleteAsync(Guid id);

        // count total records for pagination metadata
        Task<int> CountAsync();
    }
}