namespace ConferenceBooking.Domain.Exceptions
{
    /// <summary>
    /// Виключення, що виникає при невалідних параметрах бронювання
    /// (некоректні дати, тривалість, послуги тощо).
    /// </summary>
    public class InvalidBookingException : DomainException
    {
        public InvalidBookingException(string message)
            : base(message)
        {
        }
    }
}
