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
    public class RentalController(
        IRentalService rentalService
    ) : ControllerBase
    {
        private readonly IRentalService _rentalService =
            rentalService;

        [HttpGet("{id:guid}")]
        public async Task<
            ActionResult<ApiResponse<RentalResponseDto>>
        > GetById(Guid id)
        {
            var result =
                await _rentalService.GetByIdAsync(id);

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
            var chartererId = User.GetUserId();

            var result = await _rentalService.GetAllAsync(chartererId);

            return Ok(
                ApiResponse<List<RentalResponseDto>>.SuccessResult(
                    result,
                    "Rental requests retrieved successfully"
                )
            );
        }

        [Authorize]
        [HttpPost]
        public async Task<
            ActionResult<ApiResponse<RentalStatusResponseDto>>
        > Create(
            [FromBody] CreateRentalDto dto
        )
        {
            var chartererId =
                User.GetUserId();

            var result =
                await _rentalService.CreateAsync(
                    dto,
                    chartererId
                );

            return StatusCode(
                StatusCodes.Status201Created,
                ApiResponse<RentalStatusResponseDto>
                    .SuccessResult(
                        result,
                        "Rental request created successfully"
                    )
            );
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<
            ActionResult<ApiResponse<RentalStatusResponseDto>>> Update(Guid id,[FromBody] UpdateRentalDto dto)
        {
            var result =
                await _rentalService.UpdateAsync(
                    id,
                    dto
                );

            return Ok(
                ApiResponse<RentalStatusResponseDto>.SuccessResult(
                    result,
                    "Rental request updated successfully"
                )
            );
        }

        [Authorize]
        [HttpPatch("{id:guid}/cancel")]
        public async Task<
            ActionResult<ApiResponse<object>>> Cancel(Guid id)
        {
            var chartererId =
                User.GetUserId();

            await _rentalService.CancelAsync(
                id,
                chartererId
            );

            return Ok(
                ApiResponse<object>.SuccessResult(
                    null!,
                    "Rental request cancelled successfully"
                )
            );
        }

        [HttpPost("estimate")]
        public async Task<ActionResult<ApiResponse<RentalEstimateResponseDto>>> Estimate(
            [FromBody] EstimateRentalDto dto
        )
        {
            var result =
                await _rentalService.EstimateAsync(dto);

            return Ok(
                ApiResponse<RentalEstimateResponseDto>.SuccessResult(
                    result,
                    "Rental estimate calculated successfully"
                )
            );
        }
    }
}
