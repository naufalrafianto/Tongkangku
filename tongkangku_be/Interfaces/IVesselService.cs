using tongkangku_be.Dtos.VesselRequest;

namespace tongkangku_be.Interfaces
{
    public interface IVesselService
    {
        Task <VesselResponseDto> CreateVesselAsync(VesselRequestDto request);
        Task<List<VesselResponseDto>> GetAllVesselAsync(string? search, int page, int limit);
        Task<List<VesselResponseDto>> GetAllVesselByOwnerAsync(Guid ownerId, string? search, int limit, int page);
        Task<VesselResponseDto> GetVesselById(Guid id);
    }
}
