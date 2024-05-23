using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using DepositCalculator.API.Utils;
using DepositCalculator.Shared.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DepositCalculator.API.Extensions
{
    /// <summary>
    /// Extension methods for setting up services in an <see cref="IServiceCollection" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds cross-origin resource sharing services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">
        /// The <see cref="IConfiguration"/> to provide configuration for cross-origin resource sharing services.
        /// </param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection corsSection = configuration.GetSection(ApiCorsOptions.SectionName);

            services.Configure<ApiCorsOptions>(corsSection);

            return services.AddCors(options =>
            {
                ApiCorsOptions apiCorsOptions = corsSection.Get<ApiCorsOptions>();

                options.AddPolicy(PolicyNames.SpecificOrigins, policyOpt =>
                {
                    policyOpt
                        .WithOrigins(apiCorsOptions.AllowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });

                options.AddPolicy(PolicyNames.SpecificPaginationResponseHeaders, policyOpt =>
                {
                    policyOpt
                        .WithOrigins(apiCorsOptions.AllowedOrigins)
                        .WithMethods(HttpMethod.Get.Method)
                        .WithExposedHeaders(ResponseHeaderConstants.TotalCount);
                });
            });
        }
    }
}