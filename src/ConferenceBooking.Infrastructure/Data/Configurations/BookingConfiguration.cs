using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Enums;

namespace ConferenceBooking.Infrastructure.Data.Configurations
{
    /// <summary>EF Core конфігурація таблиці Bookings.</summary>
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.TotalPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            // many-to-many Booking <-> Service через join-таблицю
            builder.HasMany(b => b.SelectedServices)
                .WithMany(s => s.Bookings)
                .UsingEntity("BookingService");

            builder.HasOne(b => b.ConferenceRoom)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.ConferenceRoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // DurationInHours — обчислювана, не зберігається в БД
            builder.Ignore(b => b.DurationInHours);
        }
    }
}
