using System.Diagnostics.Contracts;

using WMSCommon.Contracts;
using WMSCommon.Entities;
using WMSCommon.Results;

namespace WMSCommon.Services
{
    public interface IGenericSyncService<TGenericEntity> where TGenericEntity : class, ISyncEntity
    {
        public Task<RepositoryResult<TGenericEntity>> CreateAndPublishAsync<TEvent>(TGenericEntity entity)
            where TEvent : ISyncEvent<TGenericEntity>, new();
        public Task<RepositoryResult<TGenericEntity>> UpdateAndPublishAsync<TEvent>(TGenericEntity entity)
            where TEvent : ISyncEvent<TGenericEntity>, new();
        public Task<RepositoryResult<TGenericEntity>> DeleteAndPublishAsync<TEvent>(Guid id)
            where TEvent : ISyncEvent<TGenericEntity>, new();
    }
}
