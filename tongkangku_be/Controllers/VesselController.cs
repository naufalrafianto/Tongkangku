using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos.VesselRequest;
using tongkangku_be.Interfaces;

namespace tongkangku_be.Controllers
{
    [ApiController]
    [Route("api/vessel")]
    public class VesselController : ControllerBase
    {
        private readonly IVesselService _vesselService;

        public VesselController(IVesselService vesselService)
        {
            _vesselService = vesselService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateVesselAsync(VesselRequestDto request)
        {
            var result = await _vesselService.CreateVesselAsync(request);
            if (result == null)
            {
                return BadRequest(new { message = "error", error = "gagal menambahkan vessel!" });
            }
            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllVesselAsync()
        {
            var result = await _vesselService.GetAllVesselAsync();
            if (result == null)
            {
                return BadRequest(new { message = "error", error = "Data kosong!" });
            }
            return Ok(result);
        }
    }
}
