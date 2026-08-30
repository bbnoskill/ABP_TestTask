using Microsoft.EntityFrameworkCore;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Infrastructure.Data
{
    /// <summary>Контекст бази даних конференц-букінгу.</summary>
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ConferenceRoom> ConferenceRooms => Set<ConferenceRoom>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<RoomService> RoomServices => Set<RoomService>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            Seed.DataSeeder.Seed(modelBuilder);
        }
    }
}
