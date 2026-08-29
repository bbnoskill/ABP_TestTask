namespace ConferenceBooking.Domain.Exceptions
{
    /// <summary>Невалідні параметри бронювання.</summary>
    public class InvalidBookingException : DomainException
    {
        public InvalidBookingException(string message) : base(message) { }
    }
}
