using tongkangku_be.Dtos.CargoType;

namespace tongkangku_be.Interfaces
{
    public interface ICargoTypeService
    {
        Task<List<CargoTypeResponseDto>> GetAllAsync();

    }
}
