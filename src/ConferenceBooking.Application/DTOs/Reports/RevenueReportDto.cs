namespace ConferenceBooking.Application.DTOs.Reports
{
    /// <summary>DTO звіту доходів.</summary>
    public class RevenueReportDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal AverageBookingPrice { get; set; }
        public int TotalBookings { get; set; }
        public List<RoomRevenueDto> RevenueByRoom { get; set; } = new();
    }

    public class RoomRevenueDto
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int BookingsCount { get; set; }
    }
}
