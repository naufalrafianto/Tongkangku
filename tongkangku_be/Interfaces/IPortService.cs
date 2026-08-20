using tongkangku_be.Dtos.PortRequest;

namespace tongkangku_be.Interfaces
{
    public interface IPortService
    {
        Task<PortResponseDto> GetPortByIdAsync(Guid id);
        Task<PortResponseDto> CreatePortAsync(PortRequestDto dto);
        Task DeletePortAsync(Guid id);
        Task<List<PortResponseDto>> GetAllPortAsync();
        
    }
}
