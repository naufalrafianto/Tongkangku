namespace tongkangku_be.Dtos.RentalContract
{
    public class ContractCargoResponseDto
    {
        public Guid Id { get; set; }
        public Guid CargoTypeId { get; set; }
        public string CargoName { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal? FreightRatePerTon { get; set; }
    }
}
