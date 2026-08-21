using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos.CargoType;
using tongkangku_be.Dtos.PortRequest;
using tongkangku_be.Interfaces;
using tongkangku_be.Shared;

namespace tongkangku_be.Controllers
{
    [ApiController]
    [Route("/api/cargo-types")]
    public class CargoTypeController(ICargoTypeService cargoTypeService) :ControllerBase
    {
        private readonly ICargoTypeService _cargoTypeService = cargoTypeService;


        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CargoTypeResponseDto>>>> GetAllAsync()
        {
            var result = await _cargoTypeService.GetAllAsync();
            if (result == null || result.Count == 0)
            {
                return NotFound(
                    ApiResponse<List<CargoTypeResponseDto>>.ErrorResult(
                        "Cargo type data not found.",
                        "CARGO_TYPE_NOT_FOUND"
                    )
                );
            }

            return Ok(
                   ApiResponse<List<CargoTypeResponseDto>>.SuccessResult(
                       result,
                       "Cargo data retrieved successfully."
                   )
               );
        }
    }
}

