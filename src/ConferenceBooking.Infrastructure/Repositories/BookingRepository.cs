using Microsoft.EntityFrameworkCore;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Enums;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Infrastructure.Data;

namespace ConferenceBooking.Infrastructure.Repositories
{
    /// <summary>EF Core реалізація репозиторію бронювань.</summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _context.Bookings
                .Include(b => b.ConferenceRoom)
                .Include(b => b.SelectedServices)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.ConferenceRoom)
                .Include(b => b.SelectedServices)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByRoomIdAsync(Guid roomId)
        {
            return await _context.Bookings
                .Where(b => b.ConferenceRoomId == roomId)
                .Include(b => b.SelectedServices)
                .OrderByDescending(b => b.StartDateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByDateRangeAsync(
            DateTime startDate, DateTime endDate)
        {
            return await _context.Bookings
                .Where(b => b.StartDateTime >= startDate && b.StartDateTime <= endDate)
                .Include(b => b.ConferenceRoom)
                .Include(b => b.SelectedServices)
                .ToListAsync();
        }

        public async Task<bool> HasConflictAsync(
            Guid roomId, DateTime startDateTime, DateTime endDateTime,
            Guid? excludeBookingId = null)
        {
            var query = _context.Bookings
                .Where(b => b.ConferenceRoomId == roomId)
                .Where(b => b.Status == BookingStatus.Confirmed)
                .Where(b => b.StartDateTime < endDateTime && b.EndDateTime > startDateTime);

            if (excludeBookingId.HasValue)
                query = query.Where(b => b.Id != excludeBookingId.Value);

            return await query.AnyAsync();
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        public void Update(Booking booking)
        {
            _context.Bookings.Update(booking);
        }
    }
}
