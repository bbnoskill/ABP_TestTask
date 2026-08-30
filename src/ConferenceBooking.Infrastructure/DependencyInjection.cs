using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Infrastructure.Data;
using ConferenceBooking.Infrastructure.Repositories;

namespace ConferenceBooking.Infrastructure
{
    /// <summary>Реєстрація залежностей Infrastructure layer.</summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services, string connectionString)
        {
            // SQLite через EF Core
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            // Repositories
            services.AddScoped<IConferenceRoomRepository, ConferenceRoomRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
