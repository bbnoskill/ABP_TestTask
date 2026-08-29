namespace ConferenceBooking.Domain.Enums
{
    /// <summary>
    /// Статус бронювання конференц-залу.
    /// </summary>
    public enum BookingStatus
    {
        /// <summary>
        /// Бронювання підтверджене та активне.
        /// </summary>
        Confirmed,

        /// <summary>
        /// Бронювання скасоване.
        /// </summary>
        Cancelled
    }
}
