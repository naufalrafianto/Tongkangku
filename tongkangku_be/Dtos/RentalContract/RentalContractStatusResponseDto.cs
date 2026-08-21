using tongkangku_be.Models.Enums;

namespace tongkangku_be.Dtos.RentalContract
{
    public class RentalContractStatusResponseDto
    {
        public Guid Id { get; set; }
        public string ContractNum { get; set; } = string.Empty;
        public RentalContractStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
