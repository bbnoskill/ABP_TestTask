using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Infrastructure.Data;

namespace ConferenceBooking.Infrastructure.Repositories
{
    /// <summary>Unit of Work — обгортка над DbContext.SaveChangesAsync().</summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
