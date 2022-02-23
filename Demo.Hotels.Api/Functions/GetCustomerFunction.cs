using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Demo.Hotels.Api.Application;
using Demo.Hotels.Api.DTO.Requests;
using Demo.Hotels.Api.DTO.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Demo.Hotels.Api.Functions
{
    public class GetCustomerFunction
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<GetCustomerFunction> _logger;

        public GetCustomerFunction(ICustomerService customerService, ILogger<GetCustomerFunction> log)
        {
            _customerService = customerService;
            _logger = log;
        }

        [FunctionName(nameof(GetCustomerFunction))]
        [OpenApiOperation(operationId: "GetCustomer", tags: new[] { "Customers" })]
        [OpenApiParameter(name: "customerId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **customerId** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(GetCustomerResponse), Description = "The OK response")]
        public async Task<IActionResult> GetCustomer(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/customers/{customerId}")] HttpRequest req, string customerId)
        {
            var customerResponse = await _customerService.GetCustomer(new GetCustomerRequest
            {
                CustomerId = customerId,
                CorrelationId = Guid.NewGuid().ToString("N")
            });

            return new OkObjectResult(customerResponse);
        }
    }
}

