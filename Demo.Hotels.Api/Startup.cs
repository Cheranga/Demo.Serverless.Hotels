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
            services.AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<IOptionsSnapshot<EmailConfig>>().Value;
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
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            // services.AddSingleton<ICustomerService, CustomerService>();

            services.AddSingleton<ICancelHotelReservationService, CancelHotelReservationService>();
            services.AddHttpClient<ICustomerApiService, CustomerApiService>();
            services.AddSingleton<ICommandHandler<CancelReservationCommand>, CancelReservationCommandHandler>();
            services.AddSingleton<IEmailService, EmailService>();
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