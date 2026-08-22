using Microsoft.EntityFrameworkCore;
using tongkangku_be.Data;
using tongkangku_be.Interfaces;
using tongkangku_be.Models;

namespace tongkangku_be.Repositories
{
    public class LaytimeRecordRepository(ApplicationDbContext context) : Repository<LaytimeRecord>(context), ILaytimeRecordRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<LaytimeRecord>> GetByContractIdAsync(
            Guid contractId,
            params string[] includes)
        {
            IQueryable<LaytimeRecord> query = _context.LaytimeRecords;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query
                .Where(x => x.ContractId == contractId)
                .ToListAsync();
        }
    }
}
