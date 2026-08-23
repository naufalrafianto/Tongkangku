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
            try
            {
                var result = await _portService.GetPortByIdAsync(id);
                return Ok(ApiResponse<PortResponseDto>.SuccessResult(
                    result,$"Get Data {id} Successfully!"));
            }catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResult(
                    ex.Message,
                    "DATA_NOT_FOUND"
                    ));
            }           
        }

        [HttpPost]
        public async Task<IActionResult> CreatePortAsync([FromBody] PortRequestDto dto)
        {
            try
            {
                var result = await _portService.CreatePortAsync(dto);
                return Ok(ApiResponse<PortResponseDto>.SuccessResult(
                    result,
                    "succes Create Port!"
                    ));
            }catch(AppException  ex){
                return BadRequest(ApiResponse<object>.ErrorResult(
                    ex.Message,
                    "BAD_REQUEST"
                    ));
            }

        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePortAsync(Guid id)
        {
            try
            {
                await _portService.DeletePortAsync(id);
                return Ok(ApiResponse<PortResponseDto>.SuccessResult(
                    data : null,
                    $"deleted data {id} successfully"

                    ));
            }catch(NotFoundException ex) {
                return NotFound(ApiResponse<object>.ErrorResult(ex.Message, "DATA_NOT_FOUND"));
                }
           
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
