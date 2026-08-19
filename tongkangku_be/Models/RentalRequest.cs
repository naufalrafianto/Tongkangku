using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tongkangku_be.Models.Enums;

namespace tongkangku_be.Models
{
<<<<<<< HEAD
    [Table("RentalRequest")]
    public class RentalRequest
    {
        [Key]
        public Guid id { get; set; }

        public Guid vesselId { get; set; }

        [ForeignKey(nameof(vesselId))]
        public Vessel? vessel { get; set; }

        public Guid chartererId { get; set; }

        [ForeignKey(nameof(chartererId))]
        public User? charterer { get; set; }

        public DateTime startDate { get; set; }

        public int planDay { get; set; }

        public decimal totalEstimatedPrice { get; set; }

        public RentalRequestStatus status { get; set; }

        public string? notes { get; set; }

        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;

=======
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

        [ForeignKey(nameof(ChartererId))]
        public User? Charterer { get; set; }
        [ForeignKey(nameof(VesselId))]
        public Vessel? Vessel { get; set; }
        [ForeignKey(nameof(RentalContractId))]
>>>>>>> 9e5c8529687f7ba2e72a0bb092c178674af3e937
        public RentalContract? RentalContract { get; set; }
    }
}
