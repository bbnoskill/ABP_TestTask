using ConferenceBooking.Domain.Enums;

namespace ConferenceBooking.Application.Interfaces
{
    /// <summary>Сервіс розрахунку вартості оренди.</summary>
    public interface IPricingService
    {
        decimal CalculateTotalPrice(decimal baseHourlyRate, DateTime startDateTime,
            DateTime endDateTime, IEnumerable<decimal>? servicePrices = null);
        TimeSlotType GetTimeSlotType(int hour);
        decimal GetPriceCoefficient(TimeSlotType slotType);
    }
}
