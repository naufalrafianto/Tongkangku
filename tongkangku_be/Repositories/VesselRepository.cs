using Microsoft.EntityFrameworkCore;
using tongkangku_be.Data;
using tongkangku_be.Dtos.VesselRequest;
using tongkangku_be.Interfaces;
using tongkangku_be.Models;

namespace tongkangku_be.Repositories
{
    public class VesselRepository : Repository<Vessel>, IVesselRepository
    {
        private readonly ApplicationDbContext _context;

        public VesselRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Vessel>> GetAllByOwnerAsync(Guid ownerId)
        {
            return await _context.Vessels
                .Where(v => v.OwnerId == ownerId)
                .ToListAsync();
        }
    }
}