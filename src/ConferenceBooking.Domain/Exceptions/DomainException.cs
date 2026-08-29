namespace ConferenceBooking.Domain.Exceptions
{
    /// <summary>
    /// Базовий клас для всіх доменних виключень.
    /// Використовується для відокремлення бізнес-помилок від інфраструктурних.
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message)
            : base(message)
        {
        }

        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
