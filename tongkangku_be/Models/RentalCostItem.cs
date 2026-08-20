using System.ComponentModel.DataAnnotations.Schema;
using tongkangku_be.Models.Enums;

namespace tongkangku_be.Models
{
    [Table("rental_cost_items")]
    public class RentalCostItem
    {
        public Guid Id { get; set; }

        public Guid RentalRequestId { get; set; }

        public RentalRequest RentalRequest { get; set; } = null!;

        public CostType CostType { get; set; }

        public CostBearer Bearer { get; set; }

        public decimal Amount { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
