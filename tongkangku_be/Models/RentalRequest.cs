using System.ComponentModel.DataAnnotations.Schema;
using tongkangku_be.Models.Enums;

namespace tongkangku_be.Models
{
    [Table("rental_requests")]
    public class RentalRequest
    {
        public Guid Id { get; set; }

        public Guid VesselId { get; set; }
        public Vessel Vessel { get; set; } = null!;

        public Guid ChartererId { get; set; }
        public User Charterer { get; set; } = null!;

        public CharterType CharterType { get; set; }

        public Guid LoadingPortId { get; set; }
        public Port LoadingPort { get; set; } = null!;

        public Guid DischargingPortId { get; set; }
        public Port DischargingPort { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public int PlanDay { get; set; }

        // Initial estimation
        public decimal BaseHirePrice { get; set; }

        public decimal DurationMultiplier { get; set; }

        public decimal EstimatedCost { get; set; }

        public decimal TotalEstimatedPrice { get; set; }

        public decimal TargetMargin { get; set; }

        public RentalRequestStatus Status { get; set; }

        public string? RejectionReason { get; set; }

        public string Notes { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public ICollection<RentalCostItem> CostItems { get; set; }
    = new List<RentalCostItem>();

        public ICollection<RentalRequestCargo> Cargos { get; set; }
            = new List<RentalRequestCargo>();

        public ICollection<RentalOffer> Offers { get; set; }
            = new List<RentalOffer>();

        public RentalContract? RentalContract { get; set; }

    }
}