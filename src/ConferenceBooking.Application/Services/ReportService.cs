using ConferenceBooking.Domain.Enums;
using ConferenceBooking.Domain.Interfaces;
using ConferenceBooking.Application.Interfaces;

namespace ConferenceBooking.Application.Services
{
    /// <summary>Генерація бізнес-звітів: використання залів, доходи, популярність послуг.</summary>
    public class ReportService : IReportService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IConferenceRoomRepository _roomRepository;
        private const int WorkingHoursPerDay = 17; // 06:00–23:00

        public ReportService(
            IBookingRepository bookingRepository,
            IConferenceRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<RoomUsageReportItem>> GetRoomUsageReportAsync(
            DateTime startDate, DateTime endDate)
        {
            var bookings = (await _bookingRepository.GetByDateRangeAsync(startDate, endDate))
                .Where(b => b.Status == BookingStatus.Confirmed).ToList();

            var rooms = await _roomRepository.GetAllAsync();
            var totalAvailableHours = (endDate - startDate).TotalDays * WorkingHoursPerDay;

            return rooms.Select(room =>
            {
                var roomBookings = bookings.Where(b => b.ConferenceRoomId == room.Id).ToList();
                var totalHours = roomBookings.Sum(b => b.DurationInHours);

                return new RoomUsageReportItem
                {
                    RoomId = room.Id,
                    RoomName = room.Name,
                    TotalBookings = roomBookings.Count,
                    TotalHoursBooked = Math.Round(totalHours, 1),
                    OccupancyPercentage = totalAvailableHours > 0
                        ? Math.Round(totalHours / totalAvailableHours * 100, 1) : 0
                };
            });
        }

        public async Task<RevenueReportResult> GetRevenueReportAsync(
            DateTime startDate, DateTime endDate)
        {
            var bookings = (await _bookingRepository.GetByDateRangeAsync(startDate, endDate))
                .Where(b => b.Status == BookingStatus.Confirmed).ToList();

            var rooms = await _roomRepository.GetAllAsync();
            var roomsDict = rooms.ToDictionary(r => r.Id, r => r.Name);

            var totalRevenue = bookings.Sum(b => b.TotalPrice);

            return new RevenueReportResult
            {
                TotalRevenue = totalRevenue,
                AverageBookingPrice = bookings.Count > 0
                    ? Math.Round(totalRevenue / bookings.Count, 2) : 0m,
                TotalBookings = bookings.Count,
                RevenueByRoom = bookings
                    .GroupBy(b => b.ConferenceRoomId)
                    .Select(g => new RoomRevenueItem
                    {
                        RoomId = g.Key,
                        RoomName = roomsDict.GetValueOrDefault(g.Key, "Невідомий"),
                        Revenue = g.Sum(b => b.TotalPrice),
                        BookingsCount = g.Count()
                    })
                    .OrderByDescending(r => r.Revenue)
            };
        }

        public async Task<IEnumerable<PopularServiceReportItem>> GetPopularServicesReportAsync(
            DateTime startDate, DateTime endDate)
        {
            var bookings = (await _bookingRepository.GetByDateRangeAsync(startDate, endDate))
                .Where(b => b.Status == BookingStatus.Confirmed).ToList();

            return bookings
                .SelectMany(b => b.SelectedServices)
                .GroupBy(s => s.Id)
                .Select(g =>
                {
                    var service = g.First();
                    return new PopularServiceReportItem
                    {
                        ServiceId = service.Id,
                        ServiceName = service.Name,
                        TimesOrdered = g.Count(),
                        TotalRevenue = g.Count() * service.Price
                    };
                })
                .OrderByDescending(s => s.TimesOrdered);
        }
    }
}
