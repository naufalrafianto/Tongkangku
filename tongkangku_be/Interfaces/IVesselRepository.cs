using tongkangku_be.Models;
using tongkangku_be.Repositories;

namespace tongkangku_be.Interfaces
{
    public interface IVesselRepository : IRepository<Vessel>
    {
        Task<List<Vessel>> GetAllByOwnerAsync(Guid ownerId);
    }
}