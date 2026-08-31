using Xunit;
using Moq;
using ConferenceBooking.Application.Services;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Enums;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Domain.Exceptions;

namespace ConferenceBooking.Tests.Unit.Services
{
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _bookingRepo = new();
        private readonly Mock<IConferenceRoomRepository> _roomRepo = new();
        private readonly Mock<IServiceRepository> _serviceRepo = new();
        private readonly Mock<IPricingService> _pricingService = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly BookingService _sut;

        public BookingServiceTests()
        {
            _sut = new BookingService(
                _bookingRepo.Object, _roomRepo.Object,
                _serviceRepo.Object, _pricingService.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task CreateBookingAsync_ValidData_ReturnsBooking()
        {
            var roomId = Guid.NewGuid();
            var start = DateTime.UtcNow.AddHours(1);
            var end = start.AddHours(2);

            _roomRepo.Setup(r => r.GetByIdAsync(roomId))
                .ReturnsAsync(new ConferenceRoom { Id = roomId, Name = "Test", IsActive = true, BaseHourlyRate = 1000 });
            _bookingRepo.Setup(r => r.HasConflictAsync(roomId, start, end, null))
                .ReturnsAsync(false);
            _pricingService.Setup(p => p.CalculateTotalPrice(1000, start, end, null))
                .Returns(2000);

            var result = await _sut.CreateBookingAsync(roomId, start, end);

            Assert.Equal(roomId, result.ConferenceRoomId);
            Assert.Equal(2000, result.TotalPrice);
            Assert.Equal(BookingStatus.Confirmed, result.Status);
            _bookingRepo.Verify(r => r.AddAsync(It.IsAny<Booking>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task CreateBookingAsync_StartAfterEnd_ThrowsInvalidBooking()
        {
            var start = DateTime.UtcNow.AddHours(3);
            var end = DateTime.UtcNow.AddHours(1);

            await Assert.ThrowsAsync<InvalidBookingException>(
                () => _sut.CreateBookingAsync(Guid.NewGuid(), start, end));
        }

        [Fact]
        public async Task CreateBookingAsync_RoomNotFound_ThrowsRoomNotFound()
        {
            _roomRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ConferenceRoom?)null);

            var start = DateTime.UtcNow.AddHours(1);
            var end = start.AddHours(2);

            await Assert.ThrowsAsync<RoomNotFoundException>(
                () => _sut.CreateBookingAsync(Guid.NewGuid(), start, end));
        }

        [Fact]
        public async Task CreateBookingAsync_Conflict_ThrowsBookingConflict()
        {
            var roomId = Guid.NewGuid();
            var start = DateTime.UtcNow.AddHours(1);
            var end = start.AddHours(2);

            _roomRepo.Setup(r => r.GetByIdAsync(roomId))
                .ReturnsAsync(new ConferenceRoom { Id = roomId, Name = "Test", IsActive = true });
            _bookingRepo.Setup(r => r.HasConflictAsync(roomId, start, end, null))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<BookingConflictException>(
                () => _sut.CreateBookingAsync(roomId, start, end));
        }

        [Fact]
        public async Task CancelBookingAsync_ValidBooking_SetsCancelled()
        {
            var bookingId = Guid.NewGuid();
            var booking = new Booking { Id = bookingId, Status = BookingStatus.Confirmed };

            _bookingRepo.Setup(r => r.GetByIdAsync(bookingId))
                .ReturnsAsync(booking);

            await _sut.CancelBookingAsync(bookingId);

            Assert.Equal(BookingStatus.Cancelled, booking.Status);
            _bookingRepo.Verify(r => r.Update(booking), Times.Once);
        }

        [Fact]
        public async Task CancelBookingAsync_AlreadyCancelled_Throws()
        {
            var bookingId = Guid.NewGuid();
            var booking = new Booking { Id = bookingId, Status = BookingStatus.Cancelled };

            _bookingRepo.Setup(r => r.GetByIdAsync(bookingId))
                .ReturnsAsync(booking);

            await Assert.ThrowsAsync<InvalidBookingException>(
                () => _sut.CancelBookingAsync(bookingId));
        }
    }
}
