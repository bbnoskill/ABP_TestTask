namespace ConferenceBooking.Application.DTOs.Reports
{
    /// <summary>DTO звіту використання залів.</summary>
    public class RoomUsageReportDto
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public int TotalBookings { get; set; }
        public double TotalHoursBooked { get; set; }
        public double OccupancyPercentage { get; set; }
    }
}
