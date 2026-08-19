using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    [Table("ContractCargo")]
    public class ContractCargo
    {
        [Key]
        public Guid id { get; set; }

        [Column("CONTRACT_ID")]
        public Guid contractId { get; set; }

        [ForeignKey(nameof(contractId))]
        public RentalContract? contract { get; set; }

        [Column("CARGO_TYPE_ID")]
        public Guid cargoTypeId { get; set; }

        [ForeignKey(nameof(cargoTypeId))]
        public CargoType? cargoType { get; set; }

        public string cargoName { get; set; } = string.Empty;

        public double quantity { get; set; }

        public string unit { get; set; } = string.Empty;

        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;
    }
}