using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Interfaces
{
    /// <summary>Сервіс управління бронюваннями.</summary>
    public interface IBookingService
    {
        Task<Booking?> GetByIdAsync(Guid id);
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking> CreateBookingAsync(Guid roomId, DateTime startDateTime,
            DateTime endDateTime, IEnumerable<Guid>? serviceIds = null);
        Task CancelBookingAsync(Guid id);
    }
}
