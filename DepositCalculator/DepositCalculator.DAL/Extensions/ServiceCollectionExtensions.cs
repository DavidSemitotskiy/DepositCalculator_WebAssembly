using System.Diagnostics.CodeAnalysis;
using DepositCalculator.DAL.Interfaces;
using DepositCalculator.DAL.Repositories;
using DepositCalculator.DAL.Services;
using DepositCalculator.DAL.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DepositCalculator.DAL.Extensions
{
    /// <summary>
    /// Extension methods for setting up services in an <see cref="IServiceCollection" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Data Access Layer services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">
        /// The <see cref="IConfiguration"/> to provide configuration for Data Access Layer services.
        /// </param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection databaseOptionsSection = configuration.GetSection(DepositDatabaseOptions.SectionName);

            services.Configure<DepositDatabaseOptions>(databaseOptionsSection);

            services.AddScoped<IValidatorWrapper, ValidatorWrapper>();

            services.AddScoped<IDbContextOptionsWrapper<DepositCalculatorContext>,
                SqlServerDbContextOptionsWrapper<DepositCalculatorContext>>();

            services.AddDbContext<DepositCalculatorContext>();

            services.AddScoped<IDbContextWrapper<DepositCalculatorContext>, DepositCalculatorContextWrapper>();

            return services.AddScoped<IDepositCalculationRepository, DepositCalculationRepository>();
        }
    }
}