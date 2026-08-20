using tongkangku_be.Dtos.VesselCategoryRequest;
using tongkangku_be.Interfaces;
using tongkangku_be.Models;
using tongkangku_be.Repositories;
using tongkangku_be.Shared;

namespace tongkangku_be.Services
{
    public class VesselCategoryService : IVesselCategoryService
    {
        private readonly IRepository<VesselCategory> _vesselCategoryRepository;

        public VesselCategoryService(IRepository<VesselCategory> vesselCategoryRepository)
        {
            _vesselCategoryRepository = vesselCategoryRepository;
        }

        public async Task<VesselCategoryResponseDto> CreateVesselCategoryAsync(VesselCategoryRequestDto request)
        {
            if (request == null)
            {
                return null;
            }

           
            var vesselCategory = new VesselCategory
            {
                Name = request.Name,
                Description = request.Description
            };

            
            await _vesselCategoryRepository.AddAsync(vesselCategory);
            await _vesselCategoryRepository.SaveChangesAsync();



            return new VesselCategoryResponseDto
            {
                Id = vesselCategory.Id,
                Name = vesselCategory.Name,
                Description = vesselCategory.Description
            };
        }

        public async Task<List<VesselCategoryResponseDto>> GetAllVesselCategoriesAsync()
        {
            var vesselCategories = await _vesselCategoryRepository.GetAllAsync();

            return vesselCategories.Select(c => new VesselCategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
        }

        public async Task<VesselCategoryResponseDto> GetByIdVesselCategoriesAsync(Guid id)
        {
            var vesselcategory = await _vesselCategoryRepository.GetByIdAsync(id);
            if(vesselcategory == null)
            {
              throw new NotFoundException($"Vessel category dengan ID {id} tidak ditemukan.");
            }

            return new VesselCategoryResponseDto
            {
                Id = vesselcategory.Id,
                Name = vesselcategory.Name,
                Description = vesselcategory.Description
            };
        }

        public async Task <VesselCategoryResponseDto> UpdateVesselCategoryAsync(Guid id, VesselCategoryRequestDto request)
        {
            var vesselCategories = await _vesselCategoryRepository.GetByIdAsync(id);
            if(vesselCategories == null)
            {
                throw new NotFoundException($"Vessel category dengan ID {id} tidak ditemukan.");
            }

            vesselCategories.Name = request.Name;
            vesselCategories.Description = request.Description;
             _vesselCategoryRepository.Update(vesselCategories);
            await _vesselCategoryRepository.SaveChangesAsync();

            return new VesselCategoryResponseDto
            {
                Id = vesselCategories.Id,
                Name = vesselCategories.Name,
                Description = vesselCategories.Description
            };
        }
        public async Task DeleteVesselCategoryAsync(Guid id)
        {
            var vesselCategories = await _vesselCategoryRepository.GetByIdAsync(id);
            if(vesselCategories == null)
            {
                throw new NotFoundException($"Vessel category dengan ID {id} tidak ditemukan.");
            }
            _vesselCategoryRepository.Delete(vesselCategories);
            await _vesselCategoryRepository.SaveChangesAsync();
        }
    }
}