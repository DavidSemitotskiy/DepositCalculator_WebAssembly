using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using DepositCalculator.API.Extensions;
using DepositCalculator.API.Interfaces;
using DepositCalculator.API.Services;
using DepositCalculator.API.Utils;
using DepositCalculator.BLL.Extensions;
using DepositCalculator.BLL.Mapping;
using DepositCalculator.DAL.Extensions;
using DepositCalculator.Shared.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DepositCalculator.API
{
    /// <summary>
    /// Configures the application during startup.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly Assembly _assembly;

        private readonly string _assemblyName;

        private readonly string _assemblyVersion;

        private readonly string _assemblyDescription;

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" />
        /// class using <see cref="IConfiguration" />.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration" /> of the application.</param>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

            _assembly = Assembly.GetExecutingAssembly();
            AssemblyName fullyParsedAssemblyName = _assembly.GetName();

            _assemblyName = fullyParsedAssemblyName.Name;
            _assemblyVersion = fullyParsedAssemblyName.Version.ToString();

            var descriptionAttribute = (AssemblyDescriptionAttribute)_assembly
                .GetCustomAttribute(typeof(AssemblyDescriptionAttribute));

            _assemblyDescription = descriptionAttribute.Description;
        }

        /// <summary>
        /// Configures the services of <see cref="IServiceCollection" /> that are used by the application.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to configure.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(_assemblyVersion, new OpenApiInfo
                {
                    Version = _assemblyVersion,
                    Title = _assemblyName,
                    Description = _assemblyDescription,
                });

                var xmlFilename = $"{_assemblyName}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddBusinessLogicServices();

            services.AddScoped<IDepositCalculatorOperationsService, DepositCalculatorOperationsService>();

            services.AddAutoMapper(config =>
            {
                config.AddMaps(_assembly, typeof(DepositMappingsProfile).Assembly);
            });

            services.AddValidatorsFromAssemblies(new Assembly[]
            {
                typeof(DepositInfoRequestModelValidator).Assembly
            });

            services.AddCorsPolicies(_configuration);

            services.AddDatabaseServices(_configuration);
        }

        /// <summary>
        /// Configures pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" /> to build pipeline.</param>
        /// <param name="env">
        /// The <see cref="IWebHostEnvironment" /> to retrieve information of web host environment.
        /// </param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint($"{_assemblyVersion}/swagger.json", _assemblyVersion);
                });
            }

            app.UseRouting();

            app.UseCors(PolicyNames.SpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}