using tongkangku_be.Dtos.RentalContract;
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
                UpdateAt = rental.UpdateAt,
                Cargos = rental.Cargos?.Select(c => new ContractCargoResponseDto
                {
                    Id = c.Id,
                    CargoTypeId = c.CargoTypeId,
                    CargoTypeName = c.CargoType?.Name ?? string.Empty,
                    Quantity = c.Quantity,
                    Unit = c.Unit
                }).ToList() ?? new List<ContractCargoResponseDto>()
            };
        }
        public static RentalStatusResponseDto ToStatusDto(RentalRequest rental)
        {
            return new RentalStatusResponseDto
            {
                Id = rental.Id,
                Status = rental.Status
            };
        }
    }
}
