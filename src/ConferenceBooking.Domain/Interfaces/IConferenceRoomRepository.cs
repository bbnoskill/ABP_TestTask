using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Domain.Interfaces
{
    /// <summary>
    /// Контракт репозиторію для роботи з конференц-залами.
    /// </summary>
    public interface IConferenceRoomRepository
    {
        /// <summary>
        /// Отримати конференц-зал за ідентифікатором, включаючи доступні послуги.
        /// </summary>
        Task<ConferenceRoom?> GetByIdAsync(Guid id);

        /// <summary>
        /// Отримати всі активні конференц-зали.
        /// </summary>
        Task<IEnumerable<ConferenceRoom>> GetAllAsync();

        /// <summary>
        /// Знайти доступні зали на вказаний часовий діапазон з мінімальною місткістю.
        /// Повертає зали, які не мають підтверджених бронювань в цей період.
        /// </summary>
        /// <param name="startDateTime">Початок бажаного періоду.</param>
        /// <param name="endDateTime">Кінець бажаного періоду.</param>
        /// <param name="requiredCapacity">Мінімальна необхідна місткість.</param>
        Task<IEnumerable<ConferenceRoom>> GetAvailableRoomsAsync(
            DateTime startDateTime,
            DateTime endDateTime,
            int requiredCapacity);

        /// <summary>
        /// Додати новий конференц-зал.
        /// </summary>
        Task AddAsync(ConferenceRoom room);

        /// <summary>
        /// Оновити існуючий конференц-зал.
        /// </summary>
        void Update(ConferenceRoom room);

        /// <summary>
        /// Видалити конференц-зал.
        /// </summary>
        void Delete(ConferenceRoom room);
    }
}
