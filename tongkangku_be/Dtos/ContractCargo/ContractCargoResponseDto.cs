namespace tongkangku_be.Dtos.ContractCargo
{
    public class ContractCargoResponseDto
    {
        public Guid Id { get; set; }
        public Guid CargoTypeId { get; set; }
        public string CargoTypeName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = "MT";
    }
}
