using Microsoft.EntityFrameworkCore;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Infrastructure.Data.Seed
{
    /// <summary>Наповнення БД початковими даними (зали та послуги з ТЗ).</summary>
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Фіксовані ID для детермінованого seed
            var projectorId = Guid.Parse("a1111111-1111-1111-1111-111111111111");
            var wifiId      = Guid.Parse("a2222222-2222-2222-2222-222222222222");
            var soundId     = Guid.Parse("a3333333-3333-3333-3333-333333333333");

            var roomAId = Guid.Parse("b1111111-1111-1111-1111-111111111111");
            var roomBId = Guid.Parse("b2222222-2222-2222-2222-222222222222");
            var roomCId = Guid.Parse("b3333333-3333-3333-3333-333333333333");

            // Послуги
            modelBuilder.Entity<Service>().HasData(
                new Service { Id = projectorId, Name = "Проєктор", Price = 500m, IsActive = true },
                new Service { Id = wifiId,      Name = "Wi-Fi",     Price = 300m, IsActive = true },
                new Service { Id = soundId,     Name = "Звук",      Price = 700m, IsActive = true }
            );

            // Конференц-зали
            modelBuilder.Entity<ConferenceRoom>().HasData(
                new ConferenceRoom { Id = roomAId, Name = "Зал А", Capacity = 50,  BaseHourlyRate = 2000m, IsActive = true, CreatedAt = DateTime.UtcNow },
                new ConferenceRoom { Id = roomBId, Name = "Зал B", Capacity = 100, BaseHourlyRate = 3500m, IsActive = true, CreatedAt = DateTime.UtcNow },
                new ConferenceRoom { Id = roomCId, Name = "Зал C", Capacity = 30,  BaseHourlyRate = 1500m, IsActive = true, CreatedAt = DateTime.UtcNow }
            );

            // Всі послуги доступні в кожному залі
            modelBuilder.Entity<RoomService>().HasData(
                new RoomService { ConferenceRoomId = roomAId, ServiceId = projectorId },
                new RoomService { ConferenceRoomId = roomAId, ServiceId = wifiId },
                new RoomService { ConferenceRoomId = roomAId, ServiceId = soundId },
                new RoomService { ConferenceRoomId = roomBId, ServiceId = projectorId },
                new RoomService { ConferenceRoomId = roomBId, ServiceId = wifiId },
                new RoomService { ConferenceRoomId = roomBId, ServiceId = soundId },
                new RoomService { ConferenceRoomId = roomCId, ServiceId = projectorId },
                new RoomService { ConferenceRoomId = roomCId, ServiceId = wifiId },
                new RoomService { ConferenceRoomId = roomCId, ServiceId = soundId }
            );
        }
    }
}
