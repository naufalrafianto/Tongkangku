using System.ComponentModel.DataAnnotations.Schema;
using tongkangku_be.Models.Enums;

namespace tongkangku_be.Models
{
    [Table("rental_offers")]
    public class RentalOffer
    {
        public Guid Id { get; set; }

        public Guid RentalRequestId { get; set; }

        public RentalRequest RentalRequest { get; set; } = null!;

        public Guid OwnerId { get; set; }

        public User Owner { get; set; } = null!;

        public decimal RatePerDay { get; set; }

        public decimal HireAmount { get; set; }

        public decimal BunkerAmount { get; set; }

        public decimal OtherCharges { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime ValidUntil { get; set; }

        public RentalOfferStatus Status { get; set; }

        public string? Notes { get; set; }
        public string? RejectionReason { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
