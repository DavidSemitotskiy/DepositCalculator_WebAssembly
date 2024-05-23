using DepositCalculator.BLL.Interfaces;
using DepositCalculator.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DepositCalculator.BLL.Extensions
{
    /// <summary>
    /// Extension methods for setting up services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Business Logic Layer services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services
                .AddScoped<IDepositCalculationStrategy, SimpleDepositCalculationStrategy>()
                .AddScoped<IDepositCalculationStrategy, CompoundDepositCalculationStrategy>();

            services.AddScoped<IDepositCalculationStrategyResolver, DepositCalculationStrategyResolver>();

            services.AddScoped<IDepositCalculationStrategyFactory, DepositCalculationStrategyFactory>();

            services.AddScoped<IDateTimeWrapper, DateTimeWrapper>();

            return services.AddScoped<IDepositCalculatorService, DepositCalculatorService>();
        }
    }
}