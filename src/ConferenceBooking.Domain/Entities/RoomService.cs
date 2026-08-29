namespace ConferenceBooking.Domain.Entities
{
    /// <summary>
    /// Зв'язуюча сутність для відношення many-to-many між ConferenceRoom та Service.
    /// Визначає, які послуги доступні в конкретному конференц-залі.
    /// </summary>
    public class RoomService
    {
        /// <summary>
        /// Ідентифікатор конференц-залу.
        /// </summary>
        public Guid ConferenceRoomId { get; set; }

        /// <summary>
        /// Навігаційна властивість до конференц-залу.
        /// </summary>
        public ConferenceRoom ConferenceRoom { get; set; } = null!;

        /// <summary>
        /// Ідентифікатор послуги.
        /// </summary>
        public Guid ServiceId { get; set; }

        /// <summary>
        /// Навігаційна властивість до послуги.
        /// </summary>
        public Service Service { get; set; } = null!;
    }
}
