using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Enums;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Domain.Exceptions;
using ConferenceBooking.Application.Interfaces;

namespace ConferenceBooking.Application.Services
{
    /// <summary>Створення бронювань з перевіркою конфліктів та розрахунком вартості.</summary>
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IConferenceRoomRepository _roomRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IPricingService _pricingService;
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(
            IBookingRepository bookingRepository,
            IConferenceRoomRepository roomRepository,
            IServiceRepository serviceRepository,
            IPricingService pricingService,
            IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _serviceRepository = serviceRepository;
            _pricingService = pricingService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<Booking> CreateBookingAsync(
            Guid roomId, DateTime startDateTime, DateTime endDateTime,
            IEnumerable<Guid>? serviceIds = null)
        {
            if (startDateTime >= endDateTime)
                throw new InvalidBookingException("Час початку повинен бути раніше за час завершення.");

            if (endDateTime - startDateTime < TimeSpan.FromMinutes(30))
                throw new InvalidBookingException("Мінімальна тривалість бронювання — 30 хвилин.");

            var room = await _roomRepository.GetByIdAsync(roomId)
                ?? throw new RoomNotFoundException(roomId);

            if (!room.IsActive)
                throw new InvalidBookingException($"Конференц-зал '{room.Name}' наразі недоступний.");

            var hasConflict = await _bookingRepository.HasConflictAsync(
                roomId, startDateTime, endDateTime);
            if (hasConflict)
                throw new BookingConflictException(roomId, startDateTime, endDateTime);

            // Обробка обраних послуг
            var selectedServices = new List<Service>();
            IEnumerable<decimal>? servicePrices = null;

            if (serviceIds != null && serviceIds.Any())
            {
                var services = (await _serviceRepository.GetByIdsAsync(serviceIds)).ToList();
                var foundIds = services.Select(s => s.Id).ToHashSet();
                var missingIds = serviceIds.Where(id => !foundIds.Contains(id)).ToList();

                if (missingIds.Any())
                    throw new InvalidBookingException(
                        $"Послуги з ID не знайдені: {string.Join(", ", missingIds)}");

                selectedServices = services;
                servicePrices = services.Select(s => s.Price);
            }

            var totalPrice = _pricingService.CalculateTotalPrice(
                room.BaseHourlyRate, startDateTime, endDateTime, servicePrices);

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                ConferenceRoomId = roomId,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime,
                TotalPrice = totalPrice,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTime.UtcNow,
                SelectedServices = selectedServices
            };

            await _bookingRepository.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return booking;
        }

        public async Task CancelBookingAsync(Guid id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id)
                ?? throw new InvalidBookingException($"Бронювання з ID '{id}' не знайдене.");

            if (booking.Status == BookingStatus.Cancelled)
                throw new InvalidBookingException("Бронювання вже скасоване.");

            booking.Status = BookingStatus.Cancelled;
            _bookingRepository.Update(booking);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
