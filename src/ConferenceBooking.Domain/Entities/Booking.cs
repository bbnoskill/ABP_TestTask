using ConferenceBooking.Domain.Enums;

namespace ConferenceBooking.Domain.Entities
{
    /// <summary>
    /// Сутність бронювання конференц-залу.
    /// Зберігає інформацію про час, вартість та обрані послуги.
    /// </summary>
    public class Booking
    {
        /// <summary>
        /// Унікальний ідентифікатор бронювання.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ідентифікатор заброньованого конференц-залу.
        /// </summary>
        public Guid ConferenceRoomId { get; set; }

        /// <summary>
        /// Дата та час початку бронювання (UTC).
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Дата та час завершення бронювання (UTC).
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Загальна вартість оренди (у гривнях).
        /// Включає базову ставку з урахуванням часових коефіцієнтів та обрані послуги.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Поточний статус бронювання.
        /// </summary>
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

        /// <summary>
        /// Дата та час створення бронювання (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // --- Обчислювані властивості ---

        /// <summary>
        /// Тривалість бронювання в годинах.
        /// </summary>
        public double DurationInHours => (EndDateTime - StartDateTime).TotalHours;

        // --- Навігаційні властивості ---

        /// <summary>
        /// Конференц-зал, до якого відноситься бронювання.
        /// </summary>
        public ConferenceRoom ConferenceRoom { get; set; } = null!;

        /// <summary>
        /// Послуги, обрані клієнтом для цього бронювання (many-to-many).
        /// </summary>
        public ICollection<Service> SelectedServices { get; set; } = new List<Service>();
    }
}
