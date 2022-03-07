namespace Demo.Hotels.Api.Core.Domain.Requests
{
    public class SendCancelConfirmationEmailRequest : IOperation
    {
        public string CorrelationId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string ReservationId { get; set; }
    }
}