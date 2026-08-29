namespace ConferenceBooking.Application.DTOs.Common
{
    /// <summary>DTO детального розрахунку вартості по інтервалах.</summary>
    public class PriceBreakdownDto
    {
        public decimal RentalCost { get; set; }
        public decimal ServicesCost { get; set; }
        public decimal TotalCost { get; set; }
        public List<TimeSlotCostDto> TimeSlotCosts { get; set; } = new();
    }

    public class TimeSlotCostDto
    {
        public string TimeSlotType { get; set; } = string.Empty;
        public decimal Hours { get; set; }
        public decimal Coefficient { get; set; }
        public decimal Cost { get; set; }
    }
}
