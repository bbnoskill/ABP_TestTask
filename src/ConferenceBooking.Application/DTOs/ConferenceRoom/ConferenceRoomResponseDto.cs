using ConferenceBooking.Application.DTOs.Service;

namespace ConferenceBooking.Application.DTOs.ConferenceRoom
{
    /// <summary>DTO відповіді з даними конференц-залу.</summary>
    public class ConferenceRoomResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal BaseHourlyRate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ServiceDto> AvailableServices { get; set; } = new();
    }
}
