using ConferenceBooking.Domain.Enums;

namespace ConferenceBooking.Domain.Entities
{
    /// <summary>Сутність бронювання конференц-залу.</summary>
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid ConferenceRoomId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public double DurationInHours => (EndDateTime - StartDateTime).TotalHours;

        public ConferenceRoom ConferenceRoom { get; set; } = null!;
        public ICollection<Service> SelectedServices { get; set; } = new List<Service>();
    }
}
