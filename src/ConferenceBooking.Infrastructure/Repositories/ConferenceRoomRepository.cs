using Microsoft.EntityFrameworkCore;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Enums;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Infrastructure.Data;

namespace ConferenceBooking.Infrastructure.Repositories
{
    /// <summary>EF Core реалізація репозиторію конференц-залів.</summary>
    public class ConferenceRoomRepository : IConferenceRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public ConferenceRoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ConferenceRoom?> GetByIdAsync(Guid id)
        {
            return await _context.ConferenceRooms
                .Include(r => r.AvailableServices)
                    .ThenInclude(rs => rs.Service)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<ConferenceRoom>> GetAllAsync()
        {
            return await _context.ConferenceRooms
                .Where(r => r.IsActive)
                .Include(r => r.AvailableServices)
                    .ThenInclude(rs => rs.Service)
                .ToListAsync();
        }

        public async Task<IEnumerable<ConferenceRoom>> GetAvailableRoomsAsync(
            DateTime startDateTime, DateTime endDateTime, int requiredCapacity)
        {
            return await _context.ConferenceRooms
                .Where(r => r.IsActive)
                .Where(r => r.Capacity >= requiredCapacity)
                .Where(r => !r.Bookings.Any(b =>
                    b.Status == BookingStatus.Confirmed &&
                    b.StartDateTime < endDateTime &&
                    b.EndDateTime > startDateTime))
                .Include(r => r.AvailableServices)
                    .ThenInclude(rs => rs.Service)
                .ToListAsync();
        }

        public async Task AddAsync(ConferenceRoom room)
        {
            await _context.ConferenceRooms.AddAsync(room);
        }

        public void Update(ConferenceRoom room)
        {
            _context.ConferenceRooms.Update(room);
        }

        public void Delete(ConferenceRoom room)
        {
            _context.ConferenceRooms.Remove(room);
        }
    }
}
