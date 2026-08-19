namespace tongkangku_be.Models
{
    public class ContractCargo
    {
        public Guid Id { get; set; }

        public Guid ContractId { get; set; }

        public Guid CargoTypeId { get; set; }

        public decimal? WeightTon { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation
        public RentalContract Contract { get; set; } = null!;

        public CargoType CargoType { get; set; } = null!;
    }
}
