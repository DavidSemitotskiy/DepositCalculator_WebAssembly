using Microsoft.EntityFrameworkCore;

namespace DepositCalculator.DAL.Interfaces
{
    /// <summary>
    /// A contract for a wrapper around the <see cref="DbContextOptions{TContext}" />.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the <see cref="DbContext" /> derived class to which the parameters are applied.
    /// </typeparam>
    public interface IDbContextOptionsWrapper<TContext> where TContext : DbContext
    {
        /// <summary>
        /// Applies the configured options to the specified <see cref="DbContextOptionsBuilder" />.
        /// </summary>
        /// <param name="optionsBuilder">
        /// The <see cref="DbContextOptionsBuilder" /> used to configure DbContext options.
        /// </param>
        void ApplyOptions(DbContextOptionsBuilder optionsBuilder);
    }
}