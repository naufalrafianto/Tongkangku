using tongkangku_be.Models;
using tongkangku_be.Repositories;

namespace tongkangku_be.Interfaces
{
    public interface IRentalContractRepository : IRepository<RentalContract>
    {
        Task<RentalContract?> GetByRentalRequestIdAsync(
    Guid rentalRequestId,
    params string[] includes);

        Task<bool> ExistsForRentalRequestAsync(Guid rentalRequestId);

        Task<int> CountByDatePrefixAsync(string prefix);
    }
}
