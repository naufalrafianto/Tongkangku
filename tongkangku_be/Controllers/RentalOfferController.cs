using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos.RentalOffer;
using tongkangku_be.Dtos.RentalRequest;
using tongkangku_be.Extensions;
using tongkangku_be.Interfaces;
using tongkangku_be.Shared;

namespace tongkangku_be.Controllers
{
    [ApiController]
    [Route("api/rental-offers")]
    public class RentalOfferController : ControllerBase
    {
        private readonly IRentalOfferService _rentalOfferService;

        public RentalOfferController(IRentalOfferService rentalOfferService)
        {
            _rentalOfferService = rentalOfferService;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<RentalOfferResponseDto>>> GetById(Guid id)
        {
            var result = await _rentalOfferService.GetByIdAsync(id);

            return Ok(ApiResponse<RentalOfferResponseDto>.SuccessResult(result, "Rental request updated successfully"));

        }
        [HttpGet("rental-request/{rentalRequestId:guid}")]
        public async Task<
       ActionResult<ApiResponse<List<RentalOfferResponseDto>>>> GetByRentalRequestId(Guid rentalRequestId)
        {
            var result = await _rentalOfferService
                .GetByRentalRequestIdAsync(rentalRequestId);

            return Ok(
                ApiResponse<List<RentalOfferResponseDto>>.SuccessResult(
                    result,
                    "Rental offers retrieved successfully."
                )
            );
        }


        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create([FromBody] CreateRentalOfferDto dto)

        {
            var ownerId = User.GetUserId();
            var result = await _rentalOfferService.CreateAsync(dto, ownerId);
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result
            );
        }

        [HttpPatch("{id:guid}/withdraw")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Withdraw(Guid id)
        {
            var result = await _rentalOfferService.WithdrawAsync(id);

            return Ok(result);
        }

        [HttpPatch("{id:guid}/accept")]
        [Authorize(Roles = "Charterer")]
        public async Task<IActionResult> Accept(Guid id)
        {
            var result = await _rentalOfferService.AcceptAsync(id);

            return Ok(result);
        }

        [HttpPatch("{id:guid}/reject")]
        [Authorize(Roles = "Charterer")]
        public async Task<IActionResult> Reject(
            Guid id,
            [FromBody] RejectRentalOfferDto dto)
        {
            var result = await _rentalOfferService.RejectAsync(id, dto);

            return Ok(result);
        }
    }
}