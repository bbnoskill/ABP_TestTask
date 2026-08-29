namespace ConferenceBooking.Domain.Enums
{
    /// <summary>Тип часового слоту для розрахунку вартості оренди.</summary>
    public enum TimeSlotType
    {
        Standard,  // 09:00–18:00, коефіцієнт 1.0
        Evening,   // 18:00–23:00, коефіцієнт 0.8
        Morning,   // 06:00–09:00, коефіцієнт 0.9
        Peak       // 12:00–14:00, коефіцієнт 1.15
    }
}
