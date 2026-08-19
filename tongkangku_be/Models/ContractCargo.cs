using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    [Table("contract_cargo")]
    public class ContractCargo
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ContractId { get; set; }

        [ForeignKey(nameof(ContractId))]
        public RentalContract? Contract { get; set; }

        public Guid CargoTypeId { get; set; }

        [ForeignKey(nameof(CargoTypeId))]
        public CargoType? CargoType { get; set; }

        public string CargoName { get; set; } = string.Empty;

        public double Quantity { get; set; }

        public string Unit { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}