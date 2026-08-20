using tongkangku_be.Dtos;

namespace tongkangku_be.Interfaces
{
    public interface IRentalService
    {
        Task<RentalResponseDto> GetByIdAsync(Guid id);
        Task<List<RentalResponseDto>> GetAllAsync();
        Task<RentalResponseDto> CreateAsync(CreateRentalDto dto);
        Task<RentalResponseDto> UpdateAsync(Guid id, UpdateRentalDto dto);
        Task DeleteAsync(Guid id);
        //Task<>
    }
}
