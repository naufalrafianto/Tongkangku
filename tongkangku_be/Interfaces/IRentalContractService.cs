using tongkangku_be.Dtos.RentalContract;

namespace tongkangku_be.Interfaces
{
    public interface IRentalContractService
    {
        Task<RentalContractResponseDto> GetByIdAsync(Guid id);
        Task<List<RentalContractResponseDto>> GetAllAsync();
        Task<RentalContractResponseDto> GetByRentalRequestIdAsync(Guid rentalRequestId);

        Task<RentalContractStatusResponseDto> CreateAsync(CreateRentalContractDto dto);
        Task<RentalContractStatusResponseDto> CompleteAsync(Guid id);
        Task<RentalContractStatusResponseDto> CancelAsync(Guid id);
    }
}
