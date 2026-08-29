namespace ConferenceBooking.Application.DTOs.ConferenceRoom
{
    /// <summary>DTO для створення конференц-залу.</summary>
    public class CreateConferenceRoomDto
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal BaseHourlyRate { get; set; }
        public List<Guid>? ServiceIds { get; set; }
    }
}
