using System.Threading.Tasks;
using DepositCalculator.Entities;

namespace DepositCalculator.DAL.Interfaces
{
    /// <summary>
    /// A base generic repository contract for providing basic operations for all repositories.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity managed by the repository.</typeparam>
    /// <typeparam name="TId">The type of the identifier for the entity.</typeparam>
    public interface IBaseRepository<TEntity, TId> where TEntity : BaseEntity<TId>
    {
        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Retrieves the count of entities in the database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous count operation. The result is the count of entities.
        /// </returns>
        Task<int> CountAsync();

        /// <summary>
        /// Retrieves an entity based on <paramref name="id" />.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous get by identifier operation.
        /// The result is the entity with the specified identifier, or null if no entity is found.
        /// </returns>
        Task<TEntity?> GetByIdAsync(TId id);
    }
}