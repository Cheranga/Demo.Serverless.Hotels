using Azure.Identity;
using Demo.Hotels.Api;
using Demo.Hotels.Api.Application;
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

            RegisterConfigurations(services, configuration);
            RegisterServices(services);
            RegisterAuthClients(services, configuration);
        }

        private void RegisterConfigurations(IServiceCollection services, IConfigurationRoot configuration)
        {
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
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<ICustomerService, CustomerService>();
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