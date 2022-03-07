using System.Net.Http;
using System.Threading.Tasks;
using Demo.Hotels.Api.Core;
using Demo.Hotels.Api.DTO.Requests;
using Demo.Hotels.Api.DTO.Responses;
using Microsoft.Extensions.Logging;

namespace Demo.Hotels.Api.Services
{
    public interface ICustomerApiService
    {
        Task<Result<GetCustomerResponse>> GetCustomerAsync(GetCustomerRequest request);
    }
    
    public class CustomerApiService : ICustomerApiService
    {
        private readonly HttpClient _client;
        private readonly ILogger<CustomerApiService> _logger;

        public CustomerApiService(HttpClient client, ILogger<CustomerApiService> logger)
        {
            _client = client;
            _logger = logger;
        }
        
        public Task<Result<GetCustomerResponse>> GetCustomerAsync(GetCustomerRequest request)
        {
            _logger.LogInformation("{CorrelationId}: getting customer information for {CustomerId}", request.CorrelationId, request.CustomerId);
            
            // TODO: Get the customer api response using HTTP Client
            return Task.FromResult(Result<GetCustomerResponse>.Success(new GetCustomerResponse()));
        }
    }
}