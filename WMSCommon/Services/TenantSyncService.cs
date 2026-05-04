using WMSCommon.Contracts;
using WMSCommon.Entities;
using WMSCommon.Repositories;
using WMSCommon.Results;
using Wolverine;

namespace WMSCommon.Services
{
    public class TenantSyncService<TEntity>(
        ITenantRepository<TEntity> repository,
        IMessageBus messageBus)
        : ITenantSyncService<TEntity>
        where TEntity : class, ISyncEntity, ITenantEntity
    {
        // Create
        public async Task<RepositoryResult<TEntity>> CreateAndPublishAsync<TEvent>(TEntity entity)
            where TEvent : ISyncEvent<TEntity>, new()
        {
            return await ExecuteWithTransaction<TEvent>(
                async () => await repository.CreateAsync(entity));
        }

        // Update
        public async Task<RepositoryResult<TEntity>> UpdateAndPublishAsync<TEvent>(TEntity entity)
            where TEvent : ISyncEvent<TEntity>, new()
        {
            return await ExecuteWithTransaction<TEvent>(
                async () => await repository.UpdateAsync(entity));
        }

        // Delete
        public async Task<RepositoryResult<TEntity>> DeleteAndPublishAsync<TEvent>(Guid id) 
            where TEvent : ISyncEvent<TEntity>, new()
        {
            return await ExecuteWithTransaction<TEvent>(
                async () => await repository.DeleteAsync(id));
        }

        private async Task<RepositoryResult<TEntity>> ExecuteWithTransaction<TEvent>(
            Func<Task<RepositoryResult<TEntity>>> repositoryAction)
            where TEvent : ISyncEvent<TEntity>, new()
        {
            var result = await repositoryAction();
            if (!result.IsSuccess)
                return result;

            var message = new TEvent();
            message.MapFrom(result.Data);

            await messageBus.PublishAsync(message);

            return result;
        }
    }
}