using System.ComponentModel.DataAnnotations.Schema;
using tongkangku_be.Models.Enums;

namespace tongkangku_be.Models
{
    [Table("rental_operational_cost")]
    public class RentalOperationalCost
    {
        public Guid Id { get; set; }
        public CostType CostType { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
