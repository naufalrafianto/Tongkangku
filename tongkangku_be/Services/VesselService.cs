using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<VesselService> _logger;

        public VesselService(
            IRepository<Vessel> vesselRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Port> portRepository,
            IRepository<VesselCategory> vesselCategoryRepository,
            ILogger<VesselService> logger)
        {
            _vesselRepository = vesselRepository;
            _httpContextAccessor = httpContextAccessor;
            _portRepository = portRepository;
            _vesselCategoryRepository = vesselCategoryRepository;
            _logger = logger;
        }

        public async Task<bool> DeleteVesselAsync(Guid id)
        {
            _logger.LogInformation("Memproses penghapusan vessel dengan ID: {Id}", id);

            var vessel = await _vesselRepository.GetByIdAsync(id);
            if (vessel == null)
            {
                _logger.LogWarning("Penghapusan gagal. Vessel dengan ID {Id} tidak ditemukan", id);
                throw new NotFoundException($"Vessel dengan ID {id} tidak ditemukan");
            }

            _vesselRepository.Delete(vessel);
            await _vesselRepository.SaveChangesAsync();

            _logger.LogInformation("Berhasil menghapus vessel ID: {Id}", id);
            return true;
        }

        public async Task<List<VesselResponseDto>> GetMyVessel()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                ?? _httpContextAccessor.HttpContext?.User?.FindFirst("id");

            if (userIdClaim == null)
            {
                _logger.LogWarning("user harus login dulu!");
                throw new UnauthorizedAccessException("User belum login");
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                _logger.LogWarning("Format user ID di dalam token tidak valid!");
                throw new UnauthorizedAccessException("Format id tidak UUID!");
            }

            var allVessels = await _vesselRepository.GetAllAsync();

            var myVessels = allVessels
                .Where(v => v.OwnerId == userId)
                .Select(v => new VesselResponseDto
                {
                    Id = v.Id,
                    name = v.Name,
                    ownerId = v.OwnerId,
                    categoryId = v.CategoryId,
                    portId = v.PortId,
                    capacityFeed = v.CapacityFeed,
                    dwtCapacity = v.DwtCapacity,
                    status = (int)v.Status,
                    year = v.Year,
                    ratePerDay = v.RatePerDay,
                    createdAt = v.CreatedAt,
                }).ToList();

            _logger.LogInformation("Vessel Berhasil diambil!");
            return myVessels;
        }

        public async Task<VesselResponseDto> CreateVesselAsync(VesselRequestDto request)
        {
            if (request == null)
            {
                _logger.LogWarning("request data vessel create kosong!");
                throw new AppException("Request tidak boleh kosong", System.Net.HttpStatusCode.BadRequest);
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                ?? _httpContextAccessor.HttpContext?.User?.FindFirst("id");

            if (userIdClaim == null)
            {
                _logger.LogWarning("user harus login dulu!");
                throw new UnauthorizedAccessException("User belum login");
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("Format id tidak UUID!");
            }

            var port = await _portRepository.GetByIdAsync(request.portId);
            if (port == null)
            {
                _logger.LogWarning("Port id tidak ada! {id}", request.portId);
                throw new NotFoundException("port id tidak ada!");
            }

            var category = await _vesselCategoryRepository.GetByIdAsync(request.categoryId);
            if (category == null)
            {
                _logger.LogWarning("category Id tidak ada: {id}", request.categoryId);
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
                DwtCapacity = request.dwtCapacity,
                Status = (VesselStatus)request.status,
                CreatedAt = DateTime.UtcNow,
            };

            await _vesselRepository.AddAsync(vessel);
            await _vesselRepository.SaveChangesAsync();

            _logger.LogInformation("succes create vessel: {name}, ditambahakan oleh {ownerId}", vessel.Name, vessel.OwnerId);

            return new VesselResponseDto
            {
                Id = vessel.Id,
                name = vessel.Name,
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

        public async Task<List<VesselResponseDto>> GetAllVesselByOwnerAsync(Guid ownerId, string? search, int limit, int page)
        {
            var vessels = await _vesselRepository.GetAllAsync();

            var ownerVessels = vessels.Where(v => v.OwnerId == ownerId).ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                ownerVessels = ownerVessels
                    .Where(v => v.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return ownerVessels
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(v => new VesselResponseDto
                {
                    Id = v.Id,
                    name = v.Name,
                    ownerId = v.OwnerId,
                    categoryId = v.CategoryId,
                    portId = v.PortId,
                    capacityFeed = v.CapacityFeed,
                    dwtCapacity = v.DwtCapacity,
                    status = (int)v.Status,
                    year = v.Year,
                    ratePerDay = v.RatePerDay,
                    createdAt = v.CreatedAt,
                })
                .ToList();
        }

        public async Task<List<VesselResponseDto>> GetAllVesselAsync(string? search, int limit, int page)
        {
            var vessel = await _vesselRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(search))
            {
                vessel = vessel
                    .Where(v => v.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var vesselPages = vessel
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(v => new VesselResponseDto
                {
                    Id = v.Id,
                    name = v.Name,
                    ownerId = v.OwnerId,
                    categoryId = v.CategoryId,
                    portId = v.PortId,
                    capacityFeed = v.CapacityFeed,
                    dwtCapacity = v.DwtCapacity,
                    status = (int)v.Status,
                    year = v.Year,
                    ratePerDay = v.RatePerDay,
                    createdAt = v.CreatedAt,
                }).ToList();

            return vesselPages ?? new List<VesselResponseDto>();
        }

        public async Task<VesselResponseDto> GetVesselById(Guid id)
        {
            var vessel = await _vesselRepository.GetByIdAsync(id);
            if (vessel == null)
            {
                throw new NotFoundException("Vessel tidak ditemukan.");
            }

            return new VesselResponseDto
            {
                Id = vessel.Id,
                name = vessel.Name,
                categoryId = vessel.CategoryId,
                ownerId = vessel.OwnerId,
                portId = vessel.PortId,
                capacityFeed = vessel.CapacityFeed,
                dwtCapacity = vessel.DwtCapacity,
                ratePerDay = vessel.RatePerDay,
                year = vessel.Year,
                status = (int)vessel.Status
            };
        }
    }
}