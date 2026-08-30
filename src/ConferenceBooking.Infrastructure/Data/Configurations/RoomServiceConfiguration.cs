using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Infrastructure.Data.Configurations
{
    /// <summary>EF Core конфігурація join-таблиці RoomServices.</summary>
    public class RoomServiceConfiguration : IEntityTypeConfiguration<RoomService>
    {
        public void Configure(EntityTypeBuilder<RoomService> builder)
        {
            // Композитний ключ
            builder.HasKey(rs => new { rs.ConferenceRoomId, rs.ServiceId });

            builder.HasOne(rs => rs.ConferenceRoom)
                .WithMany(r => r.AvailableServices)
                .HasForeignKey(rs => rs.ConferenceRoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rs => rs.Service)
                .WithMany(s => s.RoomServices)
                .HasForeignKey(rs => rs.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
