using tongkangku_be.Dtos.VesselRequest;

namespace tongkangku_be.Interfaces
{
    public interface IVesselService
    {
        Task <VesselResponseDto> CreateVesselAsync(VesselRequestDto request);
        Task<List<VesselResponseDto>> GetAllVesselAsync(string? search, int page, int limit);
        Task<VesselResponseDto> GetVesselById(Guid id);
    }
}
