using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Data.Tables;
using Azure.Identity;
using Demo.Hotels.Api;
using Demo.Hotels.Api.Core;
using Demo.Hotels.Api.Features.CancelReservation;
using Demo.Hotels.Api.Infrastructure;
using Demo.Hotels.Api.Infrastructure.CustomerApi;
using Demo.Hotels.Api.Infrastructure.DataAccess;
using Demo.Hotels.Api.Infrastructure.Email;
using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly:FunctionsStartup(typeof(Startup))]
namespace Demo.Hotels.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = GetConfiguration(builder);
            var services = builder.Services;

            RegisterValidators(services);
            RegisterConfigurations(services, configuration);
            RegisterServices(services);
            RegisterAuthClients(services, configuration);
        }

        private void RegisterValidators(IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(ModelValidatorBase<>).Assembly);
        }

        private void RegisterConfigurations(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<EmailConfig>(configuration.GetSection(nameof(EmailConfig)));
            services.Configure<TableConfig>(configuration.GetSection(nameof(TableConfig)));

            services.AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<IOptionsSnapshot<EmailConfig>>().Value;
                return config;
            });
            
            services.AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<IOptionsSnapshot<TableConfig>>().Value;
                return config;
            });
        }

        private void RegisterAuthClients(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddAzureClients(builder =>
            {
                builder.AddQueueServiceClient(configuration.GetSection("QueueSource")).WithCredential(new DefaultAzureCredential(new DefaultAzureCredentialOptions
                {
                    ExcludeEnvironmentCredential = true,
                    ExcludeAzurePowerShellCredential = true,
                    ExcludeInteractiveBrowserCredential = true,
                    ExcludeVisualStudioCredential = false,
                    ExcludeManagedIdentityCredential = false
                }));

                var tableUri = configuration.GetValue<string>("Values:TableConfig__tableServiceUri");
                
                
                builder.AddTableServiceClient(new Uri(tableUri)).WithCredential(new DefaultAzureCredential(new DefaultAzureCredentialOptions
                {
                    ExcludeEnvironmentCredential = true,
                    ExcludeAzurePowerShellCredential = true,
                    ExcludeInteractiveBrowserCredential = true,
                    ExcludeVisualStudioCredential = false,
                    ExcludeManagedIdentityCredential = false
                }));
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            // services.AddSingleton<ICustomerService, CustomerService>();

            services.AddSingleton<ICancelHotelReservationService, CancelHotelReservationService>();
            services.AddHttpClient<ICustomerApiService, CustomerApiService>();
            services.AddSingleton<ICommandHandler<CancelReservationCommand>, CancelReservationCommandHandler>();
            services.AddSingleton<IEmailService, EmailService>();

            services.AddSingleton<ITableStorageFactory>(provider =>
            {
                var config = provider.GetRequiredService<TableConfig>();
                var tableNames = config.TableNames;
                var tables = tableNames.Split(",", StringSplitOptions.RemoveEmptyEntries);

                var mappedTables = new Dictionary<string, TableClient>();
                foreach (var tableName in tables)
                {
                    if (!mappedTables.ContainsKey(tableName))
                    {
                        var tableClient = new TableClient(new Uri(config.TableServiceUri), tableName, new DefaultAzureCredential());
                        tableClient.CreateIfNotExists();

                        mappedTables.Add(tableName.ToUpper(), tableClient);
                    }
                }

                return new TableStorageFactory(mappedTables);
            });
        }
        
        protected virtual IConfigurationRoot GetConfiguration(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;
            var executionContextOptions = services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>().Value;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(executionContextOptions.AppDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }
    }
}