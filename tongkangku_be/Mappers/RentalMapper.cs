using tongkangku_be.Dtos.RentalRequest;
using tongkangku_be.Models;

namespace tongkangku_be.Mappers
{
    public class RentalMapper
    {
        public static RentalResponseDto ToDto(RentalRequest rental) 
        {
            return new RentalResponseDto
            {
                Id = rental.Id,

                VesselId = rental.VesselId,
                VesselName = rental.Vessel?.Name,

                ChartererId = rental.ChartererId,
                ChartererName = rental.Charterer?.Name,

                StartDate = rental.StartDate,
                PlanDay = rental.PlanDay,
                TotalEstimatedPrice = rental.TotalEstimatedPrice,
                Status = rental.Status,
                RejectionReason = rental.RejectionReason,
                Notes = rental.Notes,

                CreatedAt = rental.CreatedAt,
                UpdateAt = rental.UpdateAt
            };
        }
    }
}
