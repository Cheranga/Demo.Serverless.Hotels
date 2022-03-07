using System;
using System.Net.Http;
using System.Threading.Tasks;
using Demo.Hotels.Api.Config;
using Demo.Hotels.Api.Constants;
using Demo.Hotels.Api.Core;
using Demo.Hotels.Api.DTO.Requests;
using Demo.Hotels.Api.DTO.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.Hotels.Api.Services
{
    public interface ICustomerApiService
    {
        Task<Result<GetCustomerResponse>> GetCustomerAsync(GetCustomerRequest request);
    }

    public class CustomerApiService : ICustomerApiService
    {
        private readonly HttpClient _client;
        private readonly EmailConfig _emailConfig;
        private readonly ILogger<CustomerApiService> _logger;

        public CustomerApiService(EmailConfig emailConfig, HttpClient client, ILogger<CustomerApiService> logger)
        {
            _emailConfig = emailConfig;
            _client = client;
            _logger = logger;
        }

        public async Task<Result<GetCustomerResponse>> GetCustomerAsync(GetCustomerRequest request)
        {
            _logger.LogInformation("{CorrelationId}: getting customer information for {CustomerId}", request.CorrelationId, request.CustomerId);

            try
            {
                var url = $"{_emailConfig.BaseUrl}/users/{request.CustomerId}";
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);

                var httpResponse = await _client.SendAsync(httpRequest);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return Result<GetCustomerResponse>.Failure(ErrorCodes.CannotGetCustomerData, ErrorMessages.CannotGetCustomerData);
                }

                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<GetCustomerResponse>(responseContent);

                if (response != null)
                {
                    return Result<GetCustomerResponse>.Success(response);
                }

                return Result<GetCustomerResponse>.Failure(ErrorCodes.InvalidCustomerDataReceived, ErrorMessages.InvalidCustomerDataReceived);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "{CorrelationId}: error occurred when getting customer data: {CustomerId}", request.CustomerId);
            }

            return Result<GetCustomerResponse>.Failure(ErrorCodes.ApiError, ErrorMessages.ApiError);
        }
    }
}