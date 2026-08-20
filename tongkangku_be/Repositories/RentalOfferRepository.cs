using Microsoft.EntityFrameworkCore;
using tongkangku_be.Data;
using tongkangku_be.Interfaces;
using tongkangku_be.Models;
using tongkangku_be.Models.Enums;

namespace tongkangku_be.Repositories
{
    public class RentalOfferRepository(ApplicationDbContext context) : Repository<RentalOffer>(context), IRentalOfferRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<RentalOffer>> GetByRentalRequestIdAsync(Guid rentalRequestId, params string[] includes)
       
        {
            IQueryable<RentalOffer> query = _context.RentalOffers
                .Where(x => x.RentalRequestId == rentalRequestId);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> HasActiveOfferAsync(Guid rentalRequestId, Guid ownerId)
        {
            return await _context.RentalOffers.AnyAsync(x =>
                x.RentalRequestId == rentalRequestId &&
                x.OwnerId == ownerId &&
                x.Status == RentalOfferStatus.Pending);
        }
    }
}