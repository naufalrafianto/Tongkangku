using tongkangku_be.Dtos.CargoType;
using tongkangku_be.Dtos.PortRequest;
using tongkangku_be.Interfaces;
using tongkangku_be.Models;
using tongkangku_be.Repositories;

namespace tongkangku_be.Services
{
    public class CargoTypeService(IRepository<CargoType> cargoTypeRepository): ICargoTypeService
    {
        IRepository<CargoType> _cargoTypeRepository = cargoTypeRepository;

        public async Task<List<CargoTypeResponseDto>> GetAllAsync()
        {
            var ports = await _cargoTypeRepository.GetAllAsync();

            if (ports == null)
            {
                throw new KeyNotFoundException("Port tidak ditemukan.");
            }

            return ports.Select(port => new CargoTypeResponseDto
            {
                Id = port.Id,
                Name = port.Name,
               Description= port.Description,
            }).ToList();
        }
    }
}
