using tongkangku_be.Dtos;

namespace tongkangku_be.Interfaces
{
    public interface IVesselCategoryService
    {
        Task<VesselCategoryResponseDto> CreateVesselCategoryAsync(VesselCategoryRequestDto request);
        Task<List<VesselCategoryResponseDto>> GetAllVesselCategoriesAsync();
        Task<VesselCategoryResponseDto> GetByIdVesselCategoriesAsync(Guid id);
        Task <VesselCategoryResponseDto> UpdateVesselCategoryAsync(Guid id, VesselCategoryRequestDto request);
        Task DeleteVesselCategoryAsync(Guid id);
    }
}
