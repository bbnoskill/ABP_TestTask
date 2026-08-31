using Xunit;
using Moq;
using ConferenceBooking.Application.Services;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Domain.Exceptions;

namespace ConferenceBooking.Tests.Unit.Services
{
    public class ConferenceRoomServiceTests
    {
        private readonly Mock<IConferenceRoomRepository> _roomRepo = new();
        private readonly Mock<IServiceRepository> _serviceRepo = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly ConferenceRoomService _sut;

        public ConferenceRoomServiceTests()
        {
            _sut = new ConferenceRoomService(_roomRepo.Object, _serviceRepo.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ReturnsRoom()
        {
            var room = await _sut.CreateAsync("Зал А", 50, 2000);

            Assert.Equal("Зал А", room.Name);
            Assert.Equal(50, room.Capacity);
            Assert.Equal(2000, room.BaseHourlyRate);
            Assert.True(room.IsActive);
            _roomRepo.Verify(r => r.AddAsync(It.IsAny<ConferenceRoom>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Theory]
        [InlineData("", 50, 2000)]
        [InlineData("  ", 50, 2000)]
        [InlineData("Зал", 0, 2000)]
        [InlineData("Зал", -1, 2000)]
        [InlineData("Зал", 50, 0)]
        [InlineData("Зал", 50, -100)]
        public async Task CreateAsync_InvalidData_ThrowsInvalidBooking(
            string name, int capacity, decimal rate)
        {
            await Assert.ThrowsAsync<InvalidBookingException>(
                () => _sut.CreateAsync(name, capacity, rate));
        }

        [Fact]
        public async Task UpdateAsync_RoomNotFound_ThrowsRoomNotFound()
        {
            _roomRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ConferenceRoom?)null);

            await Assert.ThrowsAsync<RoomNotFoundException>(
                () => _sut.UpdateAsync(Guid.NewGuid(), name: "Нова назва"));
        }

        [Fact]
        public async Task UpdateAsync_ValidData_UpdatesFields()
        {
            var roomId = Guid.NewGuid();
            var room = new ConferenceRoom { Id = roomId, Name = "Старий", Capacity = 10, BaseHourlyRate = 500 };

            _roomRepo.Setup(r => r.GetByIdAsync(roomId)).ReturnsAsync(room);

            var result = await _sut.UpdateAsync(roomId, name: "Новий", capacity: 100);

            Assert.Equal("Новий", result.Name);
            Assert.Equal(100, result.Capacity);
            Assert.Equal(500, result.BaseHourlyRate); // не змінювали
            Assert.NotNull(result.UpdatedAt);
        }

        [Fact]
        public async Task DeleteAsync_ValidRoom_SetsInactive()
        {
            var roomId = Guid.NewGuid();
            var room = new ConferenceRoom { Id = roomId, IsActive = true };

            _roomRepo.Setup(r => r.GetByIdAsync(roomId)).ReturnsAsync(room);

            await _sut.DeleteAsync(roomId);

            Assert.False(room.IsActive);
            Assert.NotNull(room.UpdatedAt);
        }

        [Fact]
        public async Task DeleteAsync_RoomNotFound_Throws()
        {
            _roomRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ConferenceRoom?)null);

            await Assert.ThrowsAsync<RoomNotFoundException>(
                () => _sut.DeleteAsync(Guid.NewGuid()));
        }
    }
}
