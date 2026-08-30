using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Infrastructure.Data.Configurations
{
    /// <summary>EF Core конфігурація таблиці Services.</summary>
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.IsActive)
                .HasDefaultValue(true);
        }
    }
}
