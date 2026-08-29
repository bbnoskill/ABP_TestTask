using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Domain.Interfaces
{
    /// <summary>Репозиторій конференц-залів.</summary>
    public interface IConferenceRoomRepository
    {
        Task<ConferenceRoom?> GetByIdAsync(Guid id);
        Task<IEnumerable<ConferenceRoom>> GetAllAsync();
        Task<IEnumerable<ConferenceRoom>> GetAvailableRoomsAsync(
            DateTime startDateTime, DateTime endDateTime, int requiredCapacity);
        Task AddAsync(ConferenceRoom room);
        void Update(ConferenceRoom room);
        void Delete(ConferenceRoom room);
    }
}
