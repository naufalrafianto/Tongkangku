// Mappers/RentalOfferMapper.cs
using tongkangku_be.Dtos.RentalOffer;
using tongkangku_be.Models;

namespace tongkangku_be.Mappers
{
    public static class RentalOfferMapper
    {
        public static RentalOfferResponseDto ToDto(RentalOffer offer)
        {
            return new RentalOfferResponseDto
            {
                Id = offer.Id,
                RentalRequestId = offer.RentalRequestId,

                OwnerId = offer.OwnerId,
                OwnerName = offer.Owner?.Name ?? string.Empty,

                RatePerDay = offer.RatePerDay,
                HireAmount = offer.HireAmount,
                BunkerAmount = offer.BunkerAmount,
                OtherCharges = offer.OtherCharges,
                TotalPrice = offer.TotalPrice,

                ValidUntil = offer.ValidUntil,
                Status = offer.Status,
                Notes = offer.Notes,
                RejectionReason = offer.RejectionReason,

                CreatedAt = offer.CreatedAt,
                UpdatedAt = offer.UpdatedAt
            };
        }

        public static RentalOfferStatusResponseDto ToStatusDto(RentalOffer offer)
        {
            return new RentalOfferStatusResponseDto
            {
                Id = offer.Id,
                Status = offer.Status,
                UpdatedAt = offer.UpdatedAt
            };
        }
    }
}