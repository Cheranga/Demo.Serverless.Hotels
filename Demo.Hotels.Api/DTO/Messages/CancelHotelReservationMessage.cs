namespace Demo.Hotels.Api.DTO.Messages
{
    public class CancelHotelReservationMessage
    {
        public string CorrelationId { get; set; }
        public string ReservationId { get; set; }
        public int UserId { get; set; }
    }
}