using tongkangku_be.Dtos.RentalRequest;

namespace tongkangku_be.Interfaces
{
    public interface IRentalService
    {
        Task<RentalResponseDto> GetByIdAsync(Guid id);
        Task<List<RentalResponseDto>> GetAllAsync();
        Task<RentalStatusResponseDto> CreateAsync(CreateRentalDto dto);
        Task<RentalStatusResponseDto> UpdateAsync(Guid id, UpdateRentalDto dto);
        Task DeleteAsync(Guid id);
        Task<RentalStatusResponseDto> ApproveAsync(Guid id);
        Task<RentalStatusResponseDto> RejectAsync(Guid id, RejectRentalDto dto);
    }
}
