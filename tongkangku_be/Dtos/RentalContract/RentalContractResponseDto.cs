using tongkangku_be.Models.Enums;

namespace tongkangku_be.Dtos.RentalContract
{
    public class RentalContractResponseDto
    {
        public Guid Id { get; set; }
        public string ContractNum { get; set; } = string.Empty;

        public Guid RentalRequestId { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal DemurrageRate { get; set; }
        public decimal DespatchRate { get; set; }

        public decimal AgreedRatePerDay { get; set; }
        public decimal AgreedHireAmount { get; set; }
        public decimal AgreedBunkerAmount { get; set; }
        public decimal AgreedOtherCharges { get; set; }
        public decimal? AgreedTotalPrice { get; set; }
        public decimal TotalLaytimeAdjustment { get; set; }
        public decimal? FinalSettlementAmount { get; set; }
        public DateTime? CompletedAt { get; set; }
        public RentalContractStatus Status { get; set; }

        public List<ContractCargoResponseDto> Cargos { get; set; } = [];

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
