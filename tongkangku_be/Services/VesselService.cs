using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using tongkangku_be.Dtos.PortRequest;
using tongkangku_be.Dtos.VesselRequest;
using tongkangku_be.Interfaces;
using tongkangku_be.Models;
using tongkangku_be.Repositories;
using tongkangku_be.Shared;

namespace tongkangku_be.Services
{
    public class VesselService : IVesselService
    {
        private readonly IRepository<Vessel> _vesselRepository;
        private readonly IRepository<Port> _portRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<VesselCategory> _vesselCategoryRepository;

        public VesselService(IRepository<Vessel> vesselRepository, IHttpContextAccessor httpContextAccessor, IRepository<Port> portRepository, IRepository<VesselCategory> vesselCategoryRepository)
        {
            _vesselRepository = vesselRepository;
            _httpContextAccessor = httpContextAccessor;
            _portRepository = portRepository;
            _vesselCategoryRepository = vesselCategoryRepository;
        }


        public async Task<VesselResponseDto> CreateVesselAsync(VesselRequestDto request)
        {
            var userIdClaim = _httpContextAccessor
                              .HttpContext?
                              .User?
                               .FindFirst(ClaimTypes.NameIdentifier) ?? _httpContextAccessor.HttpContext?.User?
                               .FindFirst
                               ("id");

            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User belum login");
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("Format id tidak UUID!");
            }
            ;
            var port = await _portRepository.GetByIdAsync(request.portId);

            if (port == null)
            {
                throw new NotFoundException("port id tidak ada!");
            }
            var category = await _vesselCategoryRepository.GetByIdAsync(request.categoryId);
            if (category == null)
            {
                throw new NotFoundException("category id tidak ada!");
            }

            var vessel = new Vessel
            {
                Name = request.name,
                OwnerId = userId,
                CategoryId = category.Id,
                PortId = port.Id,
                CapacityFeed = request.capacityFeed,
                Year = request.year,
                RatePerDay = request.ratePerDay,
                Status = (VesselStatus)request.status,
                CreatedAt = DateTime.UtcNow,
            };

            await _vesselRepository.AddAsync(vessel);
            await _vesselRepository.SaveChangesAsync();

            return new VesselResponseDto
            {
                Id = vessel.Id,
                ownerId = vessel.OwnerId,
                categoryId = vessel.CategoryId,
                portId = vessel.PortId,
                capacityFeed = vessel.CapacityFeed,
                year = vessel.Year,
                ratePerDay = vessel.RatePerDay,
                status = (int)vessel.Status,
                createdAt = vessel.CreatedAt,
            };



        }

        public async Task<List<VesselResponseDto>> GetAllVesselAsync()
        {
            var vessel = await _vesselRepository.GetAllAsync();
            if (vessel == null)
            {
                return null;
            }
            return vessel.Select(vessel => new VesselResponseDto
            {
                Id = vessel.Id,
                name = vessel.Name,
                categoryId = vessel.CategoryId,
                ownerId = vessel.OwnerId,
                portId = vessel.PortId,
                capacityFeed = vessel.CapacityFeed,
                dwtCapacity = vessel.DwtCapacity,
                year = vessel.Year,
                ratePerDay = vessel.RatePerDay,
                status = (int)vessel.Status
            }).ToList();
        }
    }
}
