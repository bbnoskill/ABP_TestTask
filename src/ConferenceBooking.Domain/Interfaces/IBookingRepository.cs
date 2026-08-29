using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Domain.Interfaces
{
    /// <summary>Репозиторій бронювань.</summary>
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(Guid id);
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<IEnumerable<Booking>> GetByRoomIdAsync(Guid roomId);
        Task<IEnumerable<Booking>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> HasConflictAsync(Guid roomId, DateTime startDateTime, DateTime endDateTime,
            Guid? excludeBookingId = null);
        Task AddAsync(Booking booking);
        void Update(Booking booking);
    }
}
