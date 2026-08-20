using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    [Table("rental_request_cargos")]
    public class RentalRequestCargo
    {
        public Guid Id { get; set; }

        public Guid RentalRequestId { get; set; }

        public RentalRequest RentalRequest { get; set; } = null!;

        public Guid CargoTypeId { get; set; }

        public CargoType CargoType { get; set; } = null!;

        public decimal Quantity { get; set; }

        public string Unit { get; set; } = "MT";

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}