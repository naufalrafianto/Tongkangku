using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos.RentalRequest;
using tongkangku_be.Dtos.VesselRequest;
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
            var result = await _vesselService.CreateVesselAsync(request);
            if (result == null)
            {
                return BadRequest(new { message = "error", error = "gagal menambahkan vessel!" });
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<VesselResponseDto>>>> GetAllVesselAsync(string? search, int limit, int page)
        {
            var result = await _vesselService.GetAllVesselAsync(search,limit, page);
            if (result == null || result.Count == 0)
            {
                return NotFound(
                    ApiResponse<List<VesselResponseDto>>.ErrorResult(
                        "Vessel data not found.",
                        "VESSEL_NOT_FOUND"
                    )
                );
            }

            return Ok(
                   ApiResponse<List<VesselResponseDto>>.SuccessResult(
                       result,
                       "Vessel data retrieved successfully."
                   )
               );
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<VesselResponseDto>>> GetVesselById(Guid id)
        {
            var result = await _vesselService.GetVesselById(id);

            return Ok(
                ApiResponse<VesselResponseDto>.SuccessResult(
                    result,
                    "Vessel data retrieved successfully."
                )
            );
        }

    }
}
