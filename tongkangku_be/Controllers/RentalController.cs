using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos.RentalRequest;
using tongkangku_be.Extensions;
using tongkangku_be.Interfaces;
using tongkangku_be.Shared;

namespace tongkangku_be.Controllers
{
    [ApiController]
    [Route("api/rental-request")]
    public class RentalController(IRentalService rentalService):ControllerBase
    {
        private readonly IRentalService _rentalService = rentalService;

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<RentalResponseDto>>> GetById(Guid id)
        {
            var result = await _rentalService.GetByIdAsync(id);

            return Ok(
                ApiResponse<RentalResponseDto>.SuccessResult(
                    result,
                    "Rental request retrieved successfully"
                )
            );
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<RentalResponseDto>>>> GetAll()
        {
            var result = await _rentalService.GetAllAsync();

            return Ok(
                ApiResponse<List<RentalResponseDto>>.SuccessResult(
                    result,
                    "Rental requests retrieved successfully"
                )
            );
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<RentalStatusResponseDto>>> Create([FromBody] CreateRentalDto dto)
        {
            var chartererId = User.GetUserId();
            var result = await _rentalService.CreateAsync(dto, chartererId);
            return StatusCode(StatusCodes.Status201Created, ApiResponse<RentalStatusResponseDto>.SuccessResult(result, "Rental request created successfully"));
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<RentalStatusResponseDto>>> Update(Guid id, UpdateRentalDto dto)
        {
            var result = await _rentalService.UpdateAsync(id, dto);

            return Ok( ApiResponse<RentalStatusResponseDto>.SuccessResult(result, "Rental request updated successfully"));
        }
        
        [HttpPatch("{id:guid}/cancel")]
        public async Task<ActionResult<ApiResponse<object>>> Cancel(Guid id)
        {
            var chartererId = User.GetUserId();
            var result = await _rentalService.CancelAsync(id, chartererId);
            return Ok( ApiResponse<object>.SuccessResult(null!, "Rental request cancelled successfully"));
        }

        [HttpPatch("{id:guid}/approve")]
        public async Task<ActionResult<ApiResponse<RentalStatusResponseDto>>> Approve(Guid id)
        {
            var result = await _rentalService.ApproveAsync(id);
            return Ok(ApiResponse<RentalStatusResponseDto>.SuccessResult(result, "Rental request approved successfully"));
        }

        [HttpPatch("{id:guid}/reject")]
        public async Task<ActionResult<ApiResponse<RentalStatusResponseDto>>> Reject(Guid id, RejectRentalDto dto)
        {
            var result = await _rentalService.RejectAsync(id, dto);
            return Ok(ApiResponse<RentalStatusResponseDto>.SuccessResult(result, "Rental request rejected successfully"));
        }

        [HttpPost("estimate")]
        public async Task<IActionResult> Estimate([FromBody] EstimateRentalDto dto)
        {
            var result = await _rentalService.EstimateAsync(dto);
            return Ok(ApiResponse<RentalEstimateResponseDto>.SuccessResult(result, "Rental request rejected successfully"));
        }
    }
}
