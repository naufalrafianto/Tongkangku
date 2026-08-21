using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos.PortRequest;
using tongkangku_be.Interfaces;
using tongkangku_be.Shared;

namespace tongkangku_be.Controllers
{
    [ApiController]
    [Route("/api/ports")]
    public class PortController : ControllerBase
    {
        private readonly IPortService _portService;

        public PortController(IPortService portService)
        {
            _portService = portService;
        }

        [HttpGet("{id:guid}")]

        public async Task<IActionResult> GetPortByIdAsync(Guid id)
        {
            var result = await _portService.GetPortByIdAsync(id);
            if (result == null)
                return NotFound(new { status = "error", message = "Port tidak ditemukan" });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePortAsync([FromBody] PortRequestDto dto)
        {
            var result = await _portService.CreatePortAsync(dto);
            if (result == null)
                return BadRequest(new { status = "error", message = "gagal membuat pelabuhan" });
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePortAsync(Guid id)
        {
            await _portService.DeletePortAsync(id);
            return Ok(new { message = "Port berhasil dihapus" });
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<PortResponseDto>>>> GetAllPortAsync()
        {
            var result = await _portService.GetAllPortAsync();
            if (result == null || result.Count == 0)
            {
                return NotFound(
                    ApiResponse<List<PortResponseDto>>.ErrorResult(
                        "port data not found.",
                        "PORT_NOT_FOUND"
                    )
                );
            }

            return Ok(
                   ApiResponse<List<PortResponseDto>>.SuccessResult(
                       result,
                       "Port data retrieved successfully."
                   )
               );
        }
    }
}
