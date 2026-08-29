namespace ConferenceBooking.Domain.Exceptions
{
    /// <summary>Конфлікт часу бронювання.</summary>
    public class BookingConflictException : DomainException
    {
        public BookingConflictException(Guid roomId, DateTime start, DateTime end)
            : base($"Конференц-зал з ID '{roomId}' вже заброньований "
                 + $"на період з {start:dd.MM.yyyy HH:mm} до {end:dd.MM.yyyy HH:mm}.") { }
    }
}
