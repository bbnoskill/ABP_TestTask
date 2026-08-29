namespace ConferenceBooking.Domain.Entities
{
    /// <summary>Зв'язок many-to-many між ConferenceRoom та Service.</summary>
    public class RoomService
    {
        public Guid ConferenceRoomId { get; set; }
        public ConferenceRoom ConferenceRoom { get; set; } = null!;
        public Guid ServiceId { get; set; }
        public Service Service { get; set; } = null!;
    }
}
