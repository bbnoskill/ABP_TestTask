namespace ConferenceBooking.Domain.Interfaces
{
    /// <summary>Unit of Work для атомарного збереження змін.</summary>
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
