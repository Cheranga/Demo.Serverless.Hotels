using Demo.Hotels.Api;
using Demo.Hotels.Api.Application;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly:FunctionsStartup(typeof(Startup))]
namespace Demo.Hotels.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            RegisterServices(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<ICustomerService, CustomerService>();
        }
    }
}