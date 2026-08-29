using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Domain.Interfaces
{
    /// <summary>
    /// Контракт репозиторію для роботи з послугами.
    /// </summary>
    public interface IServiceRepository
    {
        /// <summary>
        /// Отримати послугу за ідентифікатором.
        /// </summary>
        Task<Service?> GetByIdAsync(Guid id);

        /// <summary>
        /// Отримати всі активні послуги.
        /// </summary>
        Task<IEnumerable<Service>> GetAllAsync();

        /// <summary>
        /// Отримати послуги за списком ідентифікаторів (для бронювання).
        /// </summary>
        /// <param name="ids">Колекція ідентифікаторів послуг.</param>
        Task<IEnumerable<Service>> GetByIdsAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// Додати нову послугу.
        /// </summary>
        Task AddAsync(Service service);

        /// <summary>
        /// Оновити існуючу послугу.
        /// </summary>
        void Update(Service service);

        /// <summary>
        /// Видалити послугу.
        /// </summary>
        void Delete(Service service);
    }
}
