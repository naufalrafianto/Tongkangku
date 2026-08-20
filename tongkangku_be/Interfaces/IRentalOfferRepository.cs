using tongkangku_be.Models;
using tongkangku_be.Repositories;

namespace tongkangku_be.Interfaces
{
    public interface IRentalOfferRepository : IRepository<RentalOffer>
    {
        Task<List<RentalOffer>> GetByRentalRequestIdAsync(Guid rentalRequestId, params string[] includes);
        Task<bool> HasActiveOfferAsync(Guid rentalRequestId, Guid ownerId);
    }
}
