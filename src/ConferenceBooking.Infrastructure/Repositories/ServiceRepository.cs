using Microsoft.EntityFrameworkCore;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Infrastructure.Data;

namespace ConferenceBooking.Infrastructure.Repositories
{
    /// <summary>EF Core реалізація репозиторію послуг.</summary>
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Service?> GetByIdAsync(Guid id)
        {
            return await _context.Services.FindAsync(id);
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _context.Services
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            var idList = ids.ToList();
            return await _context.Services
                .Where(s => idList.Contains(s.Id) && s.IsActive)
                .ToListAsync();
        }

        public async Task AddAsync(Service service)
        {
            await _context.Services.AddAsync(service);
        }

        public void Update(Service service)
        {
            _context.Services.Update(service);
        }

        public void Delete(Service service)
        {
            _context.Services.Remove(service);
        }
    }
}
