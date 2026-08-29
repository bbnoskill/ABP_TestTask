namespace ConferenceBooking.Domain.Entities
{
    /// <summary>Послуга для бронювання (проєктор, Wi-Fi, звук тощо).</summary>
    public class Service
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<RoomService> RoomServices { get; set; } = new List<RoomService>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
