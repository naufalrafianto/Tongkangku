namespace tongkangku_be.Models
{
    public class CargoType
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ICollection<ContractCargo> ContractCargos { get; set; }
            = new List<ContractCargo>();
    }
}
