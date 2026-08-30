using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Infrastructure.Data.Configurations
{
    /// <summary>EF Core конфігурація таблиці ConferenceRooms.</summary>
    public class ConferenceRoomConfiguration : IEntityTypeConfiguration<ConferenceRoom>
    {
        public void Configure(EntityTypeBuilder<ConferenceRoom> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.BaseHourlyRate)
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.IsActive)
                .HasDefaultValue(true);

            builder.HasMany(r => r.Bookings)
                .WithOne(b => b.ConferenceRoom)
                .HasForeignKey(b => b.ConferenceRoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.AvailableServices)
                .WithOne(rs => rs.ConferenceRoom)
                .HasForeignKey(rs => rs.ConferenceRoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
