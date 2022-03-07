namespace Demo.Hotels.Api.Features.CancelReservation
{
    public class CancelHotelReservationMessage
    {
        public string CorrelationId { get; set; }
        public string ReservationId { get; set; }
        public int UserId { get; set; }
    }
}