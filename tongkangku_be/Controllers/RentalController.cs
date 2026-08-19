using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos;
using tongkangku_be.Interfaces;
using tongkangku_be.Shared;

namespace tongkangku_be.Controllers
{
    [ApiController]
    [Route("api/rental")]
    public class RentalController(IRentalService rentalService):ControllerBase
    {
        private readonly IRentalService _rentalService = rentalService;

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<RentalResponseDto>>>GetById(Guid id, RentalResponseDto? result)
        {

            return Ok(
                ApiResponse<RentalResponseDto>.SuccessResult(await _rentalService.GetByIdAsync(id))
            );
        }
    }
}
