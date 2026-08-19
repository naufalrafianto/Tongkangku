using tongkangku_be.Dtos;

namespace tongkangku_be.Interfaces
{
    public interface IRentalService
    {
        Task<RentalResponseDto> GetByIdAsync(Guid id);
    }
}
