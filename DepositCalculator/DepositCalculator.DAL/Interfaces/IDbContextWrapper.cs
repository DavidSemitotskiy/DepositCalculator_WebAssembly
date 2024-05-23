using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DepositCalculator.DAL.Interfaces
{
    /// <summary>
    /// A contract for a wrapper around the <see cref="DbContext" />.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the <see cref="DbContext" /> derived class.
    /// </typeparam>
    public interface IDbContextWrapper<TContext> where TContext : DbContext
    {
        /// <summary>
        /// Saves all changes made in <typeparamref name="TContext" /> to the database.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result contains the
        /// number of state entries written to the database.
        /// </returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a <see cref="DbSet{TEntity}" /> for the specified <typeparamref name="TEntity" />.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of entity for which a <see cref="DbSet{TEntity}"/> is requested.
        /// </typeparam>
        /// <returns>
        /// A <see cref="DbSet{TEntity}" /> that can be used to query.
        /// </returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}