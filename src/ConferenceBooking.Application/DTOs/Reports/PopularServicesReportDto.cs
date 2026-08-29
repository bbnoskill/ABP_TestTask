namespace ConferenceBooking.Application.DTOs.Reports
{
    /// <summary>DTO звіту популярності послуг.</summary>
    public class PopularServicesReportDto
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int TimesOrdered { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
