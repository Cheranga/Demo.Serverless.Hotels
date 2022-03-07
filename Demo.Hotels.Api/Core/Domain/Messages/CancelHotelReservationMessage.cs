namespace Demo.Hotels.Api.Core.Domain.Messages
{
    public class CancelHotelReservationMessage
    {
        public string CorrelationId { get; set; }
        public string ReservationId { get; set; }
        public int UserId { get; set; }
    }
}