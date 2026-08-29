using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Domain.Exceptions;
using ConferenceBooking.Application.Interfaces;

namespace ConferenceBooking.Application.Services
{
    /// <summary>CRUD-операції та пошук конференц-залів.</summary>
    public class ConferenceRoomService : IConferenceRoomService
    {
        private readonly IConferenceRoomRepository _roomRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConferenceRoomService(
            IConferenceRoomRepository roomRepository,
            IServiceRepository serviceRepository,
            IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _serviceRepository = serviceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ConferenceRoom?> GetByIdAsync(Guid id)
        {
            return await _roomRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ConferenceRoom>> GetAllAsync()
        {
            return await _roomRepository.GetAllAsync();
        }

        public async Task<IEnumerable<ConferenceRoom>> GetAvailableRoomsAsync(
            DateTime startDateTime, DateTime endDateTime, int requiredCapacity)
        {
            if (startDateTime >= endDateTime)
                throw new InvalidBookingException("Час початку повинен бути раніше за час завершення.");

            if (requiredCapacity <= 0)
                throw new InvalidBookingException("Місткість повинна бути більше 0.");

            return await _roomRepository.GetAvailableRoomsAsync(
                startDateTime, endDateTime, requiredCapacity);
        }

        public async Task<ConferenceRoom> CreateAsync(
            string name, int capacity, decimal baseHourlyRate,
            IEnumerable<Guid>? serviceIds = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidBookingException("Назва залу не може бути порожньою.");
            if (capacity <= 0)
                throw new InvalidBookingException("Місткість повинна бути більше 0.");
            if (baseHourlyRate <= 0)
                throw new InvalidBookingException("Базова вартість оренди повинна бути більше 0.");

            var room = new ConferenceRoom
            {
                Id = Guid.NewGuid(),
                Name = name,
                Capacity = capacity,
                BaseHourlyRate = baseHourlyRate,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            if (serviceIds != null && serviceIds.Any())
            {
                var services = await _serviceRepository.GetByIdsAsync(serviceIds);
                var foundIds = services.Select(s => s.Id).ToHashSet();
                var missingIds = serviceIds.Where(id => !foundIds.Contains(id)).ToList();

                if (missingIds.Any())
                    throw new InvalidBookingException(
                        $"Послуги з ID не знайдені: {string.Join(", ", missingIds)}");

                foreach (var serviceId in serviceIds)
                {
                    room.AvailableServices.Add(new RoomService
                    {
                        ConferenceRoomId = room.Id,
                        ServiceId = serviceId
                    });
                }
            }

            await _roomRepository.AddAsync(room);
            await _unitOfWork.SaveChangesAsync();

            return room;
        }

        public async Task<ConferenceRoom> UpdateAsync(
            Guid id, string? name = null, int? capacity = null,
            decimal? baseHourlyRate = null, IEnumerable<Guid>? serviceIds = null)
        {
            var room = await _roomRepository.GetByIdAsync(id)
                ?? throw new RoomNotFoundException(id);

            if (name != null)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new InvalidBookingException("Назва залу не може бути порожньою.");
                room.Name = name;
            }

            if (capacity.HasValue)
            {
                if (capacity.Value <= 0)
                    throw new InvalidBookingException("Місткість повинна бути більше 0.");
                room.Capacity = capacity.Value;
            }

            if (baseHourlyRate.HasValue)
            {
                if (baseHourlyRate.Value <= 0)
                    throw new InvalidBookingException("Базова вартість оренди повинна бути більше 0.");
                room.BaseHourlyRate = baseHourlyRate.Value;
            }

            if (serviceIds != null)
            {
                if (serviceIds.Any())
                {
                    var services = await _serviceRepository.GetByIdsAsync(serviceIds);
                    var foundIds = services.Select(s => s.Id).ToHashSet();
                    var missingIds = serviceIds.Where(sid => !foundIds.Contains(sid)).ToList();

                    if (missingIds.Any())
                        throw new InvalidBookingException(
                            $"Послуги з ID не знайдені: {string.Join(", ", missingIds)}");
                }

                room.AvailableServices.Clear();
                foreach (var serviceId in serviceIds)
                {
                    room.AvailableServices.Add(new RoomService
                    {
                        ConferenceRoomId = room.Id,
                        ServiceId = serviceId
                    });
                }
            }

            room.UpdatedAt = DateTime.UtcNow;
            _roomRepository.Update(room);
            await _unitOfWork.SaveChangesAsync();

            return room;
        }

        public async Task DeleteAsync(Guid id)
        {
            var room = await _roomRepository.GetByIdAsync(id)
                ?? throw new RoomNotFoundException(id);

            room.IsActive = false;
            room.UpdatedAt = DateTime.UtcNow;

            _roomRepository.Update(room);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
