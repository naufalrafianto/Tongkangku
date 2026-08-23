namespace tongkangku_be.Dtos.RentalContract
{
    public class ContractCargoResponseDto
    {
        public Guid Id { get; set; }
        public Guid CargoTypeId { get; set; }
        public string CargoTypeName { get; set; } = string.Empty;
        public string CargoName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal? FreightRatePerTon { get; set; }
    }
}
