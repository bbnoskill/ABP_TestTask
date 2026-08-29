using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Interfaces
{
    /// <summary>Сервіс управління конференц-залами.</summary>
    public interface IConferenceRoomService
    {
        Task<ConferenceRoom?> GetByIdAsync(Guid id);
        Task<IEnumerable<ConferenceRoom>> GetAllAsync();
        Task<IEnumerable<ConferenceRoom>> GetAvailableRoomsAsync(
            DateTime startDateTime, DateTime endDateTime, int requiredCapacity);
        Task<ConferenceRoom> CreateAsync(string name, int capacity, decimal baseHourlyRate,
            IEnumerable<Guid>? serviceIds = null);
        Task<ConferenceRoom> UpdateAsync(Guid id, string? name = null, int? capacity = null,
            decimal? baseHourlyRate = null, IEnumerable<Guid>? serviceIds = null);
        Task DeleteAsync(Guid id);
    }
}
