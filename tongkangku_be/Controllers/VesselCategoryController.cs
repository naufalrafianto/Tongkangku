
using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos.VesselCategoryRequest;
using tongkangku_be.Interfaces;

namespace tongkangku_be.Controllers
{
    [ApiController]
    [Route("api/vessel-category")]
    public class VesselCategoryController : ControllerBase
    {
        private readonly IVesselCategoryService _vesselCategoryService;

        public VesselCategoryController(IVesselCategoryService vesselCategoryService)
        {
            _vesselCategoryService = vesselCategoryService;
        }

        [HttpPost("create")]

        public async Task<IActionResult> CreateVesselCategoryAsync([FromBody] VesselCategoryRequestDto request)
        {
            var result = await _vesselCategoryService.CreateVesselCategoryAsync(request);
            if (result == null)
                return BadRequest(new { status = "error", message = "Gagal membuat kategori kapal" });
            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllVesselCategoriesAsync()
        {
            var result = await _vesselCategoryService.GetAllVesselCategoriesAsync();
            return Ok(result);
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetByIdVesselCategoryAsync(Guid id)
        {
            var result = await _vesselCategoryService.GetByIdVesselCategoriesAsync(id);
            if (result == null)
                return NotFound(new { status = "error", message = $"Kategori kapal dengan id '{id}' tidak ditemukan" });
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateVesselCategoryAsync(Guid id, [FromBody] VesselCategoryRequestDto request)
        {
            var result = await _vesselCategoryService.UpdateVesselCategoryAsync(id, request);
            if (result == null)
                return NotFound(new { status = "error", message = $"Kategori kapal dengan id '{id}' tidak ditemukan" });
            return Ok(result);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _vesselCategoryService.DeleteVesselCategoryAsync(id);

            return Ok(new { status = "success", message = "Kategori kapal berhasil dihapus" });
        }
    }
}
