namespace ConferenceBooking.Domain.Entities
{
    /// <summary>
    /// Сутність послуги, яку можна додати до бронювання.
    /// Приклади: проєктор, Wi-Fi, звукове обладнання.
    /// Вартість послуги є фіксованою за одне бронювання (не залежить від тривалості).
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Унікальний ідентифікатор послуги.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Назва послуги (наприклад, "Проєктор", "Wi-Fi", "Звук").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Вартість послуги за одне бронювання (у гривнях).
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Чи доступна послуга для замовлення.
        /// Використовується для "м'якого" видалення (soft delete).
        /// </summary>
        public bool IsActive { get; set; } = true;

        // --- Навігаційні властивості ---

        /// <summary>
        /// Зали, в яких доступна ця послуга (many-to-many через RoomService).
        /// </summary>
        public ICollection<RoomService> RoomServices { get; set; } = new List<RoomService>();

        /// <summary>
        /// Бронювання, в яких обрана ця послуга (many-to-many).
        /// </summary>
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
