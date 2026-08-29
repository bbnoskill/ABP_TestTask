namespace ConferenceBooking.Application.DTOs.Booking
{
    /// <summary>DTO для створення бронювання.</summary>
    public class CreateBookingDto
    {
        public Guid RoomId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public List<Guid>? ServiceIds { get; set; }
    }
}
