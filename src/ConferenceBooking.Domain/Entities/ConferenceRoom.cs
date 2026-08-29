namespace ConferenceBooking.Domain.Entities
{
    /// <summary>Сутність конференц-залу для оренди.</summary>
    public class ConferenceRoom
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal BaseHourlyRate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<RoomService> AvailableServices { get; set; } = new List<RoomService>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
