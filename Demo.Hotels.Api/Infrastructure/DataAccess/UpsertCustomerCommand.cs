using Demo.Hotels.Api.Core;

namespace Demo.Hotels.Api.Infrastructure.DataAccess
{
    public class UpsertCustomerCommand : IOperation, ICommand
    {
        public string CorrelationId { get; set; }

        public string ReservationId { get; set; }

        public string TrackingId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public CancellationStatus Status { get; set; } = CancellationStatus.Received;
    }

    public enum CancellationStatus
    {
        Received,
        Confirmed
    }
}