using tongkangku_be.Dtos.RentalRequest;

namespace tongkangku_be.Interfaces
{
    public interface IRentalService
    {
        Task<RentalResponseDto> GetByIdAsync(Guid id);
        Task<List<RentalResponseDto>> GetAllAsync(Guid chartererId);
        Task<RentalStatusResponseDto> CreateAsync(
            CreateRentalDto dto,
            Guid chartererId
        );
        Task<RentalStatusResponseDto> UpdateAsync(
            Guid id,
            UpdateRentalDto dto
        );
        Task<RentalStatusResponseDto> CancelAsync(
            Guid id,
            Guid chartererId
        );
        Task<RentalEstimateResponseDto> EstimateAsync(
            EstimateRentalDto dto
        );
    }
}
