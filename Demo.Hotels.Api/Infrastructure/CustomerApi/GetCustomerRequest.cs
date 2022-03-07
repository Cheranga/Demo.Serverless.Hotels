using Demo.Hotels.Api.Core;

namespace Demo.Hotels.Api.Infrastructure.CustomerApi
{
    public class GetCustomerRequest : IOperation
    {
        public string CustomerId { get; set; }
        public string CorrelationId { get; set; }
    }
}