using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Domain.Interfaces
{
    /// <summary>Репозиторій послуг.</summary>
    public interface IServiceRepository
    {
        Task<Service?> GetByIdAsync(Guid id);
        Task<IEnumerable<Service>> GetAllAsync();
        Task<IEnumerable<Service>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task AddAsync(Service service);
        void Update(Service service);
        void Delete(Service service);
    }
}
