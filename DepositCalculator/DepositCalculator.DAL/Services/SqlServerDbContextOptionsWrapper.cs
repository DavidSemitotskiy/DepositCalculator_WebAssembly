using System.Diagnostics.CodeAnalysis;
using DepositCalculator.DAL.Interfaces;
using DepositCalculator.DAL.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DepositCalculator.DAL.Services
{
    /// <inheritdoc cref="IDbContextOptionsWrapper{TContext}" />
    [ExcludeFromCodeCoverage]
    public class SqlServerDbContextOptionsWrapper<TContext> : IDbContextOptionsWrapper<TContext> where TContext : DbContext
    {
        private readonly DepositDatabaseOptions _databaseOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDbContextOptionsWrapper{TContext}" /> class
        /// using configuration options of <see cref="DepositDatabaseOptions" />.
        /// </summary>
        /// <param name="options">The configuration options of <see cref="DepositDatabaseOptions" /> for database.</param>
        public SqlServerDbContextOptionsWrapper(IOptions<DepositDatabaseOptions> options)
        {
            _databaseOptions = options.Value;
        }

        /// <inheritdoc />
        public void ApplyOptions(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_databaseOptions.ConnectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly(_databaseOptions.MigrationsAssembly);
            });
        }
    }
}