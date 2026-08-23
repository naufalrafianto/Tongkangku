using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos.VesselRequest;
using tongkangku_be.Extensions;
using tongkangku_be.Interfaces;
using tongkangku_be.Shared;

namespace tongkangku_be.Controllers
{
    [ApiController]
    [Route("api/vessels")]
    public class VesselController : ControllerBase
    {
        private readonly IVesselService _vesselService;

        public VesselController(IVesselService vesselService)
        {
            _vesselService = vesselService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVesselAsync(VesselRequestDto request)
        {
            try
            {
                var result = await _vesselService.CreateVesselAsync(request);
                return Ok(ApiResponse<VesselResponseDto>.SuccessResult(
                    result,
                    "Vessel created successfully."
                ));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.ErrorResult(ex.Message, "UNAUTHORIZED"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResult(ex.Message, "DATA_NOT_FOUND"));
            }
            catch (AppException ex)
            {
              
                return BadRequest(ApiResponse<object>.ErrorResult(
                    ex.Message,
                    "BAD_REQUEST"
                ));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("An unexpected error occurred.", "INTERNAL_SERVER_ERROR"));
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<VesselResponseDto>>>> GetAllVesselAsync(string? search, int limit, int page)
        {
            try
            {
                var result = await _vesselService.GetAllVesselAsync(search, limit, page);

                if (result == null || result.Count == 0)
                {
                    return NotFound(ApiResponse<List<VesselResponseDto>>.ErrorResult(
                        "Vessel data not found.",
                        "VESSEL_NOT_FOUND"
                    ));
                }

                return Ok(ApiResponse<List<VesselResponseDto>>.SuccessResult(
                    result,
                    "Vessel data retrieved successfully."
                ));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("An unexpected error occurred.", "INTERNAL_SERVER_ERROR"));
            }
        }

        [Authorize]
        [HttpGet("own")]
        public async Task<ActionResult<ApiResponse<List<VesselResponseDto>>>> GetAllByOwner(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
                {
            var ownerId = User.GetUserId();

            var result = await _vesselService.GetAllVesselByOwnerAsync(ownerId, search, limit, page);
            return Ok(ApiResponse<List<VesselResponseDto>>.SuccessResult(
                    result,
                    "Vessel data retrieved successfully."
                ));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<VesselResponseDto>>> GetVesselById(Guid id)
        {
            try
            {
                var result = await _vesselService.GetVesselById(id);

                return Ok(ApiResponse<VesselResponseDto>.SuccessResult(
                    result,
                    "Vessel data retrieved successfully."
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResult(
                    ex.Message,
                    "VESSEL_NOT_FOUND"
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
