using Demo.Hotels.Api.Core;

namespace Demo.Hotels.Api.DTO.Responses
{
    public class GetCustomerResponse : IOperation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string CorrelationId { get; set; }
    }
}