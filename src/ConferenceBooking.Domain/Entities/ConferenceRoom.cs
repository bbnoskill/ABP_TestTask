namespace ConferenceBooking.Domain.Entities
{
    /// <summary>
    /// Сутність конференц-залу для оренди.
    /// Містить інформацію про назву, місткість, базову вартість та доступні послуги.
    /// </summary>
    public class ConferenceRoom
    {
        /// <summary>
        /// Унікальний ідентифікатор залу.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Назва залу (наприклад, "Зал А").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Максимальна місткість залу (кількість осіб).
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Базова вартість оренди за годину (у гривнях).
        /// Фінальна ціна залежить від часового слоту бронювання.
        /// </summary>
        public decimal BaseHourlyRate { get; set; }

        /// <summary>
        /// Чи доступний зал для бронювання.
        /// Використовується для "м'якого" видалення (soft delete).
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Дата та час створення запису (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Дата та час останнього оновлення (UTC). Null, якщо не оновлювався.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // --- Навігаційні властивості ---

        /// <summary>
        /// Послуги, доступні в цьому залі (many-to-many через RoomService).
        /// </summary>
        public ICollection<RoomService> AvailableServices { get; set; } = new List<RoomService>();

        /// <summary>
        /// Список бронювань цього залу.
        /// </summary>
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
