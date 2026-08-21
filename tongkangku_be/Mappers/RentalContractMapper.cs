using tongkangku_be.Dtos.RentalContract;
using tongkangku_be.Models;

namespace tongkangku_be.Mappers
{
    public static class RentalContractMapper
    {
        public static RentalContractResponseDto ToDto(RentalContract contract)
        {
            return new RentalContractResponseDto
            {
                Id = contract.Id,
                ContractNum = contract.ContractNum,

                RentalRequestId = contract.RentalRequestId,
                OwnerId = contract.OwnerId,
                OwnerName = contract.Owner?.Name ?? string.Empty,

                StartDate = contract.StartDate,
                EndDate = contract.EndDate,

                DemurrageRate = contract.DemurrageRate,
                DespatchRate = contract.DespatchRate,

                AgreedRatePerDay = contract.AgreedRatePerDay,
                AgreedHireAmount = contract.AgreedHireAmount,
                AgreedBunkerAmount = contract.AgreedBunkerAmount,
                AgreedOtherCharges = contract.AgreedOtherCharges,
                AgreedTotalPrice = contract.AgreedTotalPrice,

                Status = contract.Status,

                Cargos = contract.ContractCargos?.Select(c => new ContractCargoResponseDto
                {
                    Id = c.Id,
                    CargoTypeId = c.CargoTypeId,
                    CargoName = c.CargoName,
                    Quantity = c.Quantity,
                    Unit = c.Unit,
                    FreightRatePerTon = c.FreightRatePerTon
                }).ToList() ?? [],

                CreatedAt = contract.CreatedAt,
                UpdatedAt = contract.UpdatedAt
            };
        }

        public static RentalContractStatusResponseDto ToStatusDto(RentalContract contract)
        {
            return new RentalContractStatusResponseDto
            {
                Id = contract.Id,
                ContractNum = contract.ContractNum,
                Status = contract.Status,
                UpdatedAt = contract.UpdatedAt
            };
        }
    }
}
