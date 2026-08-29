namespace ConferenceBooking.Domain.Interfaces
{
    /// <summary>
    /// Контракт Unit of Work для координації збереження змін у базі даних.
    /// Забезпечує атомарність операцій між кількома репозиторіями.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Зберегти всі зміни, зроблені через репозиторії, як єдину транзакцію.
        /// </summary>
        /// <param name="cancellationToken">Токен скасування операції.</param>
        /// <returns>Кількість записів, змінених у базі даних.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
