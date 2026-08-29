namespace ConferenceBooking.Application.Interfaces
{
    /// <summary>Сервіс бізнес-звітів та аналітики.</summary>
    public interface IReportService
    {
        Task<IEnumerable<RoomUsageReportItem>> GetRoomUsageReportAsync(
            DateTime startDate, DateTime endDate);
        Task<RevenueReportResult> GetRevenueReportAsync(
            DateTime startDate, DateTime endDate);
        Task<IEnumerable<PopularServiceReportItem>> GetPopularServicesReportAsync(
            DateTime startDate, DateTime endDate);
    }

    public class RoomUsageReportItem
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public int TotalBookings { get; set; }
        public double TotalHoursBooked { get; set; }
        public double OccupancyPercentage { get; set; }
    }

    public class RevenueReportResult
    {
        public decimal TotalRevenue { get; set; }
        public decimal AverageBookingPrice { get; set; }
        public int TotalBookings { get; set; }
        public IEnumerable<RoomRevenueItem> RevenueByRoom { get; set; }
            = Enumerable.Empty<RoomRevenueItem>();
    }

    public class RoomRevenueItem
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int BookingsCount { get; set; }
    }

    public class PopularServiceReportItem
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int TimesOrdered { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
