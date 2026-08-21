using Microsoft.EntityFrameworkCore;
using tongkangku_be.Data;
using tongkangku_be.Interfaces;
using tongkangku_be.Models;

namespace tongkangku_be.Repositories
{
    public class RentalContractRepository(ApplicationDbContext context)
        : Repository<RentalContract>(context), IRentalContractRepository
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<RentalContract?> GetByRentalRequestIdAsync(Guid rentalRequestId, params string[] includes)
        {
            IQueryable<RentalContract> query = _context.RentalContracts
                .Where(x => x.RentalRequestId == rentalRequestId);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync();
        }
        public async Task<bool> ExistsForRentalRequestAsync(Guid rentalRequestId) =>
          await _context.RentalContracts.AnyAsync(x => x.RentalRequestId == rentalRequestId);

        public async Task<int> CountByDatePrefixAsync(string prefix) =>
            await _context.RentalContracts.CountAsync(x => x.ContractNum.StartsWith(prefix));

    }
}
