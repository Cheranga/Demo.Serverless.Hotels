using Demo.Hotels.Api.Core;

namespace Demo.Hotels.Api.DTO.Requests
{
    public class SendCancelConfirmationEmailRequest : IOperation
    {
        public string CorrelationId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string ReservationId { get; set; }
    }
}