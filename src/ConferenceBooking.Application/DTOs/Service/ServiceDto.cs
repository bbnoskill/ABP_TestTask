namespace ConferenceBooking.Application.DTOs.Service
{
    /// <summary>DTO послуги.</summary>
    public class ServiceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
