using tongkangku_be.Models;

namespace tongkangku_be.Repositories
{
    public interface IRentalRepository: IRepository<RentalRequest>
    {
        Task<bool> HasActiveRentalConflictAsync(
            Guid vesselId,
            DateTime startDate,
            DateTime endDate
            );
    }
}
