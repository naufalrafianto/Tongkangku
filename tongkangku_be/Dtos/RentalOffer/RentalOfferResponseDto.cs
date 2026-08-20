using tongkangku_be.Models.Enums;

namespace tongkangku_be.Dtos.RentalOffer
{
       public class RentalOfferResponseDto
        {
            public Guid Id { get; set; }
            public Guid RentalRequestId { get; set; }

            public Guid OwnerId { get; set; }
            public string OwnerName { get; set; } = string.Empty;

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
