
using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos.VesselCategoryRequest;
using tongkangku_be.Interfaces;
using tongkangku_be.Shared;

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
            try
            {
                var result = await _vesselCategoryService.CreateVesselCategoryAsync(request);
                return Ok(ApiResponse<VesselCategoryResponseDto>.SuccessResult(
                    result,
                    "vessel category created successfully"
                    ));
            }catch(AppException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResult(
                    ex.Message,
                    "BAD_REQUEST"
                    ));
            }
           
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult(
                    "An unexpected error occurred.",
                    "INTERNAL_SERVER_ERROR"
                ));
            }

        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllVesselCategoriesAsync()
        {
            var result = await _vesselCategoryService.GetAllVesselCategoriesAsync();
            return Ok(ApiResponse<List<VesselCategoryResponseDto>>.SuccessResult(
                result,
                "vessel categories data!"
                ));
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetByIdVesselCategoryAsync(Guid id)
        {
            try
            {
                var result = await _vesselCategoryService.GetByIdVesselCategoriesAsync(id);
                return Ok(ApiResponse<VesselCategoryResponseDto>.SuccessResult(
                    result,
                    $"succes get vessel category {id}"
                    ));
            } catch(NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResult(
                    ex.Message, "DATA_NOT_FOUND"
                    ));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult(
                    "An unexpected error occurred.",
                    "INTERNAL_SERVER_ERROR"
                ));
            }

        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateVesselCategoryAsync(Guid id, [FromBody] VesselCategoryRequestDto request)
        {
            try
            {
                var result = await _vesselCategoryService.UpdateVesselCategoryAsync(id, request);
                return Ok(ApiResponse<VesselCategoryResponseDto>.SuccessResult(
                    result,
                    $"succes update {id} "
                    ));
            }catch(NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResult(
                    ex.Message,
                    "DATA_NOT_FOUND"
                    ));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult(
                    "An unexpected error occurred.",
                    "INTERNAL_SERVER_ERROR"
                ));
            }
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _vesselCategoryService.DeleteVesselCategoryAsync(id);
                return Ok(ApiResponse<VesselCategoryResponseDto>.SuccessResult(
                    data: null,
                    message: $"Data dengan ID {id} berhasil dihapus."
                ));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResult(
                    ex.Message,
                    "DATA_NOT_FOUND"
                    ));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult(
                    "An unexpected error occurred.",
                    "INTERNAL_SERVER_ERROR"
                ));
            }

        }
    }
}
