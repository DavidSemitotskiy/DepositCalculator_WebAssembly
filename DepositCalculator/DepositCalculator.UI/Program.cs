using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Blazored.Modal;
using DepositCalculator.Shared.Models;
using DepositCalculator.Shared.Validators;
using DepositCalculator.UI.Interfaces;
using DepositCalculator.UI.Services;
using DepositCalculator.UI.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DepositCalculator.UI
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Program
    {
        /// <summary>
        /// The main method that configures and starts the application.
        /// </summary>
        /// <param name="args">
        /// Command-line arguments passed to the application.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous run operation.
        /// </returns>
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

            Assembly assembly = typeof(Program).Assembly;

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddAutoMapper(options =>
            {
                options.AddMaps(assembly);
            });

            builder.Services.AddTransient<IValidator<DepositInfoRequestModel>, DepositInfoRequestModelValidator>();

            IConfigurationSection depositCalculatorApiSection = builder.Configuration
                .GetSection(DepositCalculatorApiOptions.SectionName);

            builder.Services.Configure<DepositCalculatorApiOptions>(depositCalculatorApiSection);

            builder.Services.AddHttpClient<IDepositCalculatorHttpClientWrapper, DepositCalculatorHttpClientWrapper>();

            builder.Services.AddTransient<IDepositCalculatorApiService, DepositCalculatorApiService>();

            builder.Services.AddSingleton<IDepositCalculationStateService, DepositCalculationStateService>();

            builder.Services.AddBlazoredModal();

            await builder.Build().RunAsync();
        }
    }
}