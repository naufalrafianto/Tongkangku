using tongkangku_be.Models.Enums;

namespace tongkangku_be.Dtos.RentalOffer
{
    public class RentalOfferStatusResponseDto
    {
        public Guid Id { get; set; }
        public RentalOfferStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
