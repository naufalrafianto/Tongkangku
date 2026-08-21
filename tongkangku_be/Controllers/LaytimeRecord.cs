using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos.LaytimeRecord;
using tongkangku_be.Interfaces;

namespace tongkangku_be.Controllers
{
    
    [ApiController]
    [Route("api/laytime-records")]
    public class LaytimeRecordController(ILaytimeRecordService laytimeRecordService) : ControllerBase
    {
        private readonly ILaytimeRecordService _laytimeRecordService = laytimeRecordService;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var records = await _laytimeRecordService.GetAllAsync();
            return Ok(records);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var record = await _laytimeRecordService.GetByIdAsync(id);
            return Ok(record);
        }

        [HttpGet("contract/{contractId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetByContractId(Guid contractId)
        {
            var records = await _laytimeRecordService.GetByContractIdAsync(contractId);
            return Ok(records);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Create([FromBody] CreateLaytimeRecordDto dto)
        {
            var result = await _laytimeRecordService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLaytimeRecordDto dto)
        {
            var result = await _laytimeRecordService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _laytimeRecordService.DeleteAsync(id);
            return NoContent();
        }
    }
}
