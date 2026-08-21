using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tongkangku_be.Dtos.RentalContract;
using tongkangku_be.Interfaces;

namespace tongkangku_be.Controllers
{
    [ApiController]
    [Route("api/rental-contracts")]
    public class RentalContractController(IRentalContractService rentalContractService) : ControllerBase
    {
        private readonly IRentalContractService _rentalContractService = rentalContractService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contracts = await _rentalContractService.GetAllAsync();
            return Ok(contracts);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var contract = await _rentalContractService.GetByIdAsync(id);
            return Ok(contract);
        }

        [HttpGet("rental-request/{rentalRequestId:guid}")]
        public async Task<IActionResult> GetByRentalRequestId(Guid rentalRequestId)
        {
            var contract = await _rentalContractService.GetByRentalRequestIdAsync(rentalRequestId);
            return Ok(contract);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Create([FromBody] CreateRentalContractDto dto)
        {
            var result = await _rentalContractService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPatch("{id:guid}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            var result = await _rentalContractService.CompleteAsync(id);
            return Ok(result);
        }

        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _rentalContractService.CancelAsync(id);
            return Ok(result);
        }
    }
}
