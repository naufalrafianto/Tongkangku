using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos.PortRequest;
using tongkangku_be.Interfaces;

namespace tongkangku_be.Controllers
{
    [ApiController]
    [Route("/api/port")]
    public class PortController : ControllerBase
    {
        private readonly IPortService _portService;

        public PortController(IPortService portService)
        {
            _portService = portService;
        }

        [HttpGet("port/{id}")]

        public async Task<IActionResult> GetPortByIdAsync(Guid id)
        {
            var result = await _portService.GetPortByIdAsync(id);
            if (result == null)
                return NotFound(new { status = "error", message = "pelabuhan tidak ditemukan" });
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePortAsync([FromBody] PortRequestDto dto)
        {
            var result = await _portService.CreatePortAsync(dto);
            if (result == null)
                return BadRequest(new { status = "error", message = "gagal membuat pelabuhan" });
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePortAsync(Guid id)
        {


            await _portService.DeletePortAsync(id);
            return Ok(new { message = "Port berhasil dihapus" });
        }

        [HttpGet("port")]
        public async Task<IActionResult> GetAllPortAsync()
        {
            var result = await _portService.GetAllPortAsync();
            if (result == null)
            {
                return NotFound(new { status = "error", message = "Data port Masih Kosong!" });

            }
            return Ok(result);
        }
    }
}
