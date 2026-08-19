using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tongkangku_be.Models.Enums;

namespace tongkangku_be.Models
{
    [Table("rental_request")]
    public class RentalRequest
    {
        [Key]
        public Guid Id { get; set; }
        public Guid VesselId { get; set; }

        public Guid ChartererId { get; set; }
        public Guid RentalContractId { get; set; }
        public DateTime StartDate { get; set; }
        public int PlanDay { get; set; }
        public decimal TotalEstimatedPrice { get; set; }
        public RentalRequestStatus Status { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(VesselId))]
        public Vessel? Vessel { get; set; }

        [ForeignKey(nameof(RentalContractId))]
        public RentalContract? RentalContract { get; set; }
    }
}
