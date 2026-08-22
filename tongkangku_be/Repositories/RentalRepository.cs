using Microsoft.EntityFrameworkCore;
using tongkangku_be.Data;
using tongkangku_be.Interfaces;
using tongkangku_be.Models;
using tongkangku_be.Models.Enums;

namespace tongkangku_be.Repositories
{
    public class RentalRepository(
        ApplicationDbContext context
    ) : Repository<RentalRequest>(context), IRentalRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<RentalRequest>> GetAllByChartererIdAsync(Guid chartererId)
        {
            return await _context.RentalRequests
                .AsNoTracking()
                .Include(x => x.Vessel)
                .Include(x => x.Charterer)
                .Where(x => x.ChartererId == chartererId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> HasActiveRentalConflictAsync(
            Guid vesselId,
            DateTime startDate,
            DateTime endDate
        )
        {
            return await _context.RentalRequests
                .AsNoTracking()
                .AnyAsync(r =>
                    r.VesselId == vesselId &&
                    r.Status == RentalRequestStatus.Offered &&
                    r.StartDate < endDate &&
                    r.StartDate.AddDays(r.PlanDay) > startDate
                );
        }
    }
}