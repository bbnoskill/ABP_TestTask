namespace ConferenceBooking.Application.DTOs.ConferenceRoom
{
    /// <summary>DTO для оновлення конференц-залу (partial update).</summary>
    public class UpdateConferenceRoomDto
    {
        public string? Name { get; set; }
        public int? Capacity { get; set; }
        public decimal? BaseHourlyRate { get; set; }
        public List<Guid>? ServiceIds { get; set; }
    }
}
