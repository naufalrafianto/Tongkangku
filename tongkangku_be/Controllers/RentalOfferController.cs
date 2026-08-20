using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos.RentalOffer;
using tongkangku_be.Interfaces;

namespace tongkangku_be.Controllers
{
    [ApiController]
    [Route("api/rental-offers")]
    [Authorize]
    public class RentalOfferController : ControllerBase
    {
        private readonly IRentalOfferService _rentalOfferService;

        public RentalOfferController(IRentalOfferService rentalOfferService)
        {
            _rentalOfferService = rentalOfferService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _rentalOfferService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create(
            [FromBody] CreateRentalOfferDto dto)
        {
            var result = await _rentalOfferService.CreateAsync(dto);

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