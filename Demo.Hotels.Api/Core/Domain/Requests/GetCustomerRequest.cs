namespace Demo.Hotels.Api.Core.Domain.Requests
{
    public class GetCustomerRequest : IOperation
    {
        public string CustomerId { get; set; }
        public string CorrelationId { get; set; }
    }
}