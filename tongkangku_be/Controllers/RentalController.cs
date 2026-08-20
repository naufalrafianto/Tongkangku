using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos;
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse<RentalResponseDto>>> Create(CreateRentalDto dto)
        {
            var result = await _rentalService.CreateAsync(dto);

            return StatusCode(StatusCodes.Status201Created, ApiResponse<RentalResponseDto>.SuccessResult(result, "Rental request created successfully"));
        }

    }
}
