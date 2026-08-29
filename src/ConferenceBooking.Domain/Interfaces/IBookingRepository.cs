using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Domain.Interfaces
{
    /// <summary>
    /// Контракт репозиторію для роботи з бронюваннями.
    /// </summary>
    public interface IBookingRepository
    {
        /// <summary>
        /// Отримати бронювання за ідентифікатором, включаючи зал та обрані послуги.
        /// </summary>
        Task<Booking?> GetByIdAsync(Guid id);

        /// <summary>
        /// Отримати всі бронювання.
        /// </summary>
        Task<IEnumerable<Booking>> GetAllAsync();

        /// <summary>
        /// Отримати всі бронювання для конкретного залу.
        /// </summary>
        /// <param name="roomId">Ідентифікатор конференц-залу.</param>
        Task<IEnumerable<Booking>> GetByRoomIdAsync(Guid roomId);

        /// <summary>
        /// Отримати бронювання за часовий діапазон (для звітів).
        /// </summary>
        /// <param name="startDate">Початок періоду.</param>
        /// <param name="endDate">Кінець періоду.</param>
        Task<IEnumerable<Booking>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Перевірити, чи існує конфлікт бронювання для залу на вказаний період.
        /// </summary>
        /// <param name="roomId">Ідентифікатор конференц-залу.</param>
        /// <param name="startDateTime">Початок бажаного періоду.</param>
        /// <param name="endDateTime">Кінець бажаного періоду.</param>
        /// <param name="excludeBookingId">ID бронювання для виключення (при оновленні).</param>
        /// <returns>True, якщо існує конфлікт.</returns>
        Task<bool> HasConflictAsync(
            Guid roomId,
            DateTime startDateTime,
            DateTime endDateTime,
            Guid? excludeBookingId = null);

        /// <summary>
        /// Додати нове бронювання.
        /// </summary>
        Task AddAsync(Booking booking);

        /// <summary>
        /// Оновити існуюче бронювання.
        /// </summary>
        void Update(Booking booking);
    }
}
