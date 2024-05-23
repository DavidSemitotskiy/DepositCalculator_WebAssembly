using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using DepositCalculator.DAL.Interfaces;
using DepositCalculator.Entities;
using Microsoft.EntityFrameworkCore;

namespace DepositCalculator.DAL.Services
{
    /// <inheritdoc cref="IDbContextWrapper{DepositCalculatorContext}"/>
    [ExcludeFromCodeCoverage]
    public class DepositCalculatorContextWrapper : IDbContextWrapper<DepositCalculatorContext>
    {
        private readonly DepositCalculatorContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepositCalculatorContextWrapper" /> class
        /// using <see cref="DepositCalculatorContext" />.
        /// </summary>
        /// <param name="context">The <see cref="DepositCalculatorContext" /> instance to be wrapped.</param>
        public DepositCalculatorContextWrapper(DepositCalculatorContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets or sets <see cref="DbSet{TEntity}" /> of <see cref="DepositCalculationEntity" /> to query.
        /// </summary>
        public DbSet<DepositCalculationEntity> DepositCalculations
        {
            get => _context.DepositCalculations;

            set => _context.DepositCalculations = value;
        }

        /// <inheritdoc />
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }
    }
}