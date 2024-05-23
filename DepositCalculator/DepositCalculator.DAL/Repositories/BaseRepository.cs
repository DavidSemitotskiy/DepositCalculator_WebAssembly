using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DepositCalculator.DAL.Interfaces;
using DepositCalculator.DAL.Services;
using DepositCalculator.Entities;
using Microsoft.EntityFrameworkCore;

namespace DepositCalculator.DAL.Repositories
{
    /// <inheritdoc cref="IBaseRepository{TEntity, TId}" />
    public abstract class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> where TEntity : BaseEntity<TId>
    {
        /// <summary>
        /// The wrapper around <see cref="DepositCalculatorContext" /> used for database interaction.
        /// </summary>
        protected readonly IDbContextWrapper<DepositCalculatorContext> _databaseContext;

        /// <summary>
        /// The <see cref="DbSet{TEntity}" /> representing the <typeparamref name="TEntity" /> in the database.
        /// </summary>
        protected readonly DbSet<TEntity> _entities;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TEntity, TId}" /> class
        /// using a wrapper around <see cref="DepositCalculatorContext" />.
        /// </summary>
        /// <param name="databaseContext">
        /// The wrapper around <see cref="DepositCalculatorContext" /> used for database interaction.
        /// </param>
        public BaseRepository(IDbContextWrapper<DepositCalculatorContext> databaseContext)
        {
            _databaseContext = databaseContext;
            _entities = _databaseContext.Set<TEntity>();
        }

        /// <inheritdoc />
        public async Task AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);

            await _databaseContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public Task<int> CountAsync()
        {
            return _entities.CountAsync();
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public virtual async Task<TEntity?> GetByIdAsync(TId id)
        {
            return await _entities.FindAsync(id);
        }
    }
}