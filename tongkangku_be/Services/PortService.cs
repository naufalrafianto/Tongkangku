using tongkangku_be.Dtos.PortRequest;
using tongkangku_be.Interfaces;
using tongkangku_be.Models;
using tongkangku_be.Repositories;

namespace tongkangku_be.Services
{
    public class PortService : IPortService
    {
        private readonly IRepository<Port> _PortRepository;
        public PortService(IRepository<Port> PortRepository)
        {
            _PortRepository = PortRepository;
        }

        public async Task<PortResponseDto> GetPortByIdAsync(Guid id)
        {
            var port = await _PortRepository.GetByIdAsync(id);
            {
                if (port == null)
                {
                    throw new KeyNotFoundException("Port tidak ditemukan.");
                }

                return new PortResponseDto
                {
                    Id = port.Id,
                    Name = port.Name,
                    City = port.City,
                    Province = port.Province,
                    CreatedAt = port.CreatedAt
                };
            }

        }

        public async Task<PortResponseDto> CreatePortAsync(PortRequestDto request)
        {
            var port = new Port
            {
                City = request.City,
                Name = request.Name,
                Province = request.Province,
                CreatedAt = DateTime.UtcNow,
            };
            if (port == null)
            {
                return null;
            }

            await _PortRepository.AddAsync(port);
            await _PortRepository.SaveChangesAsync();

            return new PortResponseDto
            {
                Id = port.Id,
                Name = port.Name,
                City = port.City,
                Province = port.Province,
                CreatedAt = port.CreatedAt
            };
        }

        public async Task DeletePortAsync(Guid id)
        {
            var port = await _PortRepository.GetByIdAsync(id);
            if (port == null)
            {
                throw new Exception("Port not found");
            }
            _PortRepository.Delete(port);
            await _PortRepository.SaveChangesAsync();

           
        }

        public async Task<List<PortResponseDto>> GetAllPortAsync()
        {
            var ports = await _PortRepository.GetAllAsync();

            if(ports == null )
            {
                throw new KeyNotFoundException("Port tidak ditemukan.");
            }

            return ports.Select(port => new PortResponseDto
            {
                Id = port.Id,
                Name = port.Name,
                City = port.City,
                Province = port.Province,
                CreatedAt = port.CreatedAt
            }).ToList(); 
        }

    }
}