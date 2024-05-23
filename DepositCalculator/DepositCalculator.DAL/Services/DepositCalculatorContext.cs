using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DepositCalculator.DAL.Interfaces;
using DepositCalculator.Entities;
using DepositCalculator.Entities.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DepositCalculator.DAL.Services
{
    /// <summary>
    /// A concrete derived class of <see cref="DbContext" /> providing access to the underlying database.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DepositCalculatorContext : DbContext
    {
        private readonly IValidatorWrapper _validator;

        private readonly IDbContextOptionsWrapper<DepositCalculatorContext> _options;

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}" /> of <see cref="DepositCalculationEntity" /> used to query.
        /// </summary>
        public DbSet<DepositCalculationEntity> DepositCalculations { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepositCalculatorContext" /> class
        /// using <see cref="IDbContextOptionsWrapper{DepositCalculatorContext}" /> and <see cref="IValidatorWrapper" />.
        /// </summary>
        /// <param name="options">
        /// The <see cref="IDbContextOptionsWrapper{DepositCalculatorContext}" /> of <see cref="DepositCalculatorContext" />.
        /// </param>
        /// <param name="validator">The <see cref="IValidatorWrapper" /> used for entities validation.</param>
        public DepositCalculatorContext(IDbContextOptionsWrapper<DepositCalculatorContext> options, IValidatorWrapper validator)
        {
            _options = options;
            _validator = validator;
        }

        /// <summary>
        /// Configures the database (and other options) to be used for this context using
        /// <see cref="IDbContextOptionsWrapper{DepositCalculatorContext}" />.
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _options.ApplyOptions(optionsBuilder);
        }

        /// <summary>
        /// Configures the model using <see cref="IEntityTypeConfiguration{TEntity}" />'s.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DepositCalculationEntityTypeConfiguration).Assembly);
        }

        /// <summary>
        /// Saves all changes made in this context to the database if all changed or added entities are valid.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result contains the
        /// number of state entries written to the database.
        /// </returns>
        /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// A concurrency violation is encountered while saving to the database.
        /// A concurrency violation occurs when an unexpected number of rows are affected during save.
        /// This is usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
        /// <exception cref="ValidationException">If <see cref="EntityEntry.Entity" /> is found to be invalid.</exception>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker
                .Entries()
                .Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified)
                .Select(entry => entry.Entity);

            foreach (object entity in entities)
            {
                var validationContext = new ValidationContext(entity);

                _validator.ValidateObject(entity, validationContext, validateAllProperties: true);
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}