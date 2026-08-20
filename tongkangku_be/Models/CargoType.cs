using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    [Table("cargo_types")]
    public class CargoType
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ICollection<ContractCargo> ContractCargos { get; set; }
            = new List<ContractCargo>();

        public ICollection<RentalRequestCargo> RentalRequestCargos { get; set; }
            = new List<RentalRequestCargo>();
    }
}