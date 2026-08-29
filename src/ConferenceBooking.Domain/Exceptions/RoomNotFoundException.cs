namespace ConferenceBooking.Domain.Exceptions
{
    /// <summary>Конференц-зал не знайдений.</summary>
    public class RoomNotFoundException : DomainException
    {
        public RoomNotFoundException(Guid roomId)
            : base($"Конференц-зал з ID '{roomId}' не знайдений.") { }
    }
}
