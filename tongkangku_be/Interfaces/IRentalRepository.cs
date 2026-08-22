using tongkangku_be.Models;
using tongkangku_be.Repositories;

namespace tongkangku_be.Interfaces
{
    public interface IRentalRepository: IRepository<RentalRequest>
    {
        Task<List<RentalRequest>> GetAllByChartererIdAsync(Guid chartererId);

        Task<bool> HasActiveRentalConflictAsync(
            Guid vesselId,
            DateTime startDate,
            DateTime endDate
            );
    }
}
