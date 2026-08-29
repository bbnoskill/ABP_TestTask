using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Application.Services;
using ConferenceBooking.Application.Mapping;

namespace ConferenceBooking.Application
{
    /// <summary>Реєстрація залежностей Application layer.</summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(cfg =>
                cfg.AddProfile<MappingProfile>());

            // FluentValidation
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            // Business services
            services.AddScoped<IConferenceRoomService, ConferenceRoomService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IPricingService, PricingService>();
            services.AddScoped<IReportService, ReportService>();

            return services;
        }
    }
}
