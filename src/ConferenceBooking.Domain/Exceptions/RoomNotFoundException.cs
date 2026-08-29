namespace ConferenceBooking.Domain.Exceptions
{
    /// <summary>
    /// Виключення, що виникає коли конференц-зал не знайдений за вказаним ідентифікатором.
    /// </summary>
    public class RoomNotFoundException : DomainException
    {
        public RoomNotFoundException(Guid roomId)
            : base($"Конференц-зал з ID '{roomId}' не знайдений.")
        {
        }
    }
}
