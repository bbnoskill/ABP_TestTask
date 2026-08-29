namespace ConferenceBooking.Application.DTOs.Search
{
    /// <summary>DTO для пошуку доступних залів.</summary>
    public class AvailableRoomSearchDto
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Capacity { get; set; }
    }
}
