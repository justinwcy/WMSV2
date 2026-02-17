using MassTransit;

using Microsoft.EntityFrameworkCore;

using WMSCommon.Contracts;
using WMSCommon.Contracts.CatalogService;
using WMSCommon.Entities;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace WMSCommon.Services
{
    public class GenericSyncService<TEntity, TContext>(
        IGenericRepository<TEntity> repository,
        IPublishEndpoint publishEndpoint,
        TContext dbContext)
        : IGenericSyncService<TEntity>
        where TEntity : class, IGenericEntity, ISyncEntity
        where TContext : DbContext
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
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                RepositoryResult<TEntity> result = await repositoryAction();
                if (!result.IsSuccess)
                {
                    return result;
                }

                var message = new TEvent();
                message.MapFrom(result.Data);
                await publishEndpoint.Publish(message);

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return RepositoryResult<TEntity>.Failure(ex.Message);
            }
        }
    }
}