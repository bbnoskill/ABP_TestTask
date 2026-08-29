using ConferenceBooking.Application.DTOs.Service;

namespace ConferenceBooking.Application.DTOs.Booking
{
    /// <summary>DTO відповіді з даними бронювання.</summary>
    public class BookingResponseDto
    {
        public Guid Id { get; set; }
        public Guid ConferenceRoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public double DurationInHours { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<ServiceDto> SelectedServices { get; set; } = new();
    }
}
