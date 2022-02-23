using System;
using System.Threading.Tasks;
using Demo.Hotels.Api.Core;
using Demo.Hotels.Api.DTO.Requests;
using Demo.Hotels.Api.DTO.Responses;
using Microsoft.Extensions.Logging;

namespace Demo.Hotels.Api.Application
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
        }
        
        public async Task<Result<GetCustomerResponse>> GetCustomer(GetCustomerRequest request)
        {
            _logger.LogInformation("{CorrelationId}: getting customer data for {CustomerId}", request.CorrelationId, request.CustomerId);
            await Task.Delay(TimeSpan.FromSeconds(2));

            var customerResponse = new GetCustomerResponse
            {
                FirstName = "Cheranga",
                LastName = "Hatangala",
                Address = "Melbourne"
            };

            return Result<GetCustomerResponse>.Success(customerResponse);
        }
    }
}