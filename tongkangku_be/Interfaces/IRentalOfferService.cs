using tongkangku_be.Dtos.RentalOffer;

namespace tongkangku_be.Interfaces
{
    public interface IRentalOfferService
    {
        Task<RentalOfferResponseDto> GetByIdAsync(Guid id);
        Task<List<RentalOfferResponseDto>> GetAllAsync();
        Task<RentalOfferStatusResponseDto> CreateAsync(CreateRentalOfferDto dto, Guid ownerId);
        Task<RentalOfferStatusResponseDto> UpdateAsync(Guid id, UpdateRentalOfferDto dto);
        Task DeleteAsync(Guid id);

        Task<RentalOfferStatusResponseDto> WithdrawAsync(Guid id);
        Task<RentalOfferStatusResponseDto> AcceptAsync(Guid id);
        Task<RentalOfferStatusResponseDto> RejectAsync(Guid id, RejectRentalOfferDto dto);
    }
}
