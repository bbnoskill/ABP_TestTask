using ConferenceBooking.Domain.Enums;
using ConferenceBooking.Application.Interfaces;

namespace ConferenceBooking.Application.Services
{
    /// <summary>Розрахунок вартості оренди з урахуванням часових коефіцієнтів.</summary>
    public class PricingService : IPricingService
    {
        // Порядок важливий: Peak перевіряється перед Standard (є його підмножиною)
        private static readonly (int Start, int End, TimeSlotType Type)[] TimeSlotBoundaries =
        {
            (6,  9,  TimeSlotType.Morning),
            (12, 14, TimeSlotType.Peak),
            (9,  18, TimeSlotType.Standard),
            (18, 23, TimeSlotType.Evening)
        };

        public decimal CalculateTotalPrice(
            decimal baseHourlyRate, DateTime startDateTime,
            DateTime endDateTime, IEnumerable<decimal>? servicePrices = null)
        {
            var rentalCost = CalculateRentalCost(baseHourlyRate, startDateTime, endDateTime);
            var servicesCost = servicePrices?.Sum() ?? 0m;
            return rentalCost + servicesCost;
        }

        public TimeSlotType GetTimeSlotType(int hour)
        {
            foreach (var (start, end, type) in TimeSlotBoundaries)
            {
                if (hour >= start && hour < end)
                    return type;
            }
            return TimeSlotType.Standard;
        }

        public decimal GetPriceCoefficient(TimeSlotType slotType)
        {
            return slotType switch
            {
                TimeSlotType.Morning  => 0.9m,
                TimeSlotType.Standard => 1.0m,
                TimeSlotType.Peak     => 1.15m,
                TimeSlotType.Evening  => 0.8m,
                _ => 1.0m
            };
        }

        // Розбиває бронювання на погодинні інтервали з відповідними коефіцієнтами
        private decimal CalculateRentalCost(decimal baseHourlyRate, DateTime start, DateTime end)
        {
            var totalCost = 0m;
            var current = start;

            while (current < end)
            {
                var nextHour = new DateTime(
                    current.Year, current.Month, current.Day,
                    current.Hour, 0, 0, current.Kind).AddHours(1);

                var intervalEnd = nextHour < end ? nextHour : end;
                var intervalHours = (decimal)(intervalEnd - current).TotalHours;
                var coefficient = GetPriceCoefficient(GetTimeSlotType(current.Hour));

                totalCost += baseHourlyRate * coefficient * intervalHours;
                current = intervalEnd;
            }

            return Math.Round(totalCost, 2);
        }
    }
}
