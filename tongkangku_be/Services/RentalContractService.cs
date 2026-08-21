using System.Net;
using tongkangku_be.Data;
using tongkangku_be.Dtos.RentalContract;
using tongkangku_be.Interfaces;
using tongkangku_be.Mappers;
using tongkangku_be.Models;
using tongkangku_be.Models.Enums;
using tongkangku_be.Repositories;
using tongkangku_be.Shared;

namespace tongkangku_be.Services
{
    public class RentalContractService(IRentalContractRepository rentalContractRepository,
        IRentalOfferRepository rentalOfferRepository,
        IRentalRepository rentalRepository,
        IRepository<Vessel> vesselRepository,
        ILaytimeRecordRepository laytimeRecordRepository,
        ApplicationDbContext context) : IRentalContractService
    {
        private readonly IRentalContractRepository _rentalContractRepository = rentalContractRepository;
        private readonly IRentalOfferRepository _rentalOfferRepository = rentalOfferRepository;
        private readonly IRentalRepository _rentalRepository = rentalRepository;
        private readonly IRepository<Vessel> _vesselRepository = vesselRepository;
        private readonly ILaytimeRecordRepository _laytimeRecordRepository = laytimeRecordRepository;

        private readonly ApplicationDbContext _context = context;

        public async Task<RentalContractResponseDto> GetByIdAsync(Guid id)
        {
            var contract = await _rentalContractRepository.GetByIdAsync(id, "Owner", "Cargos");
            
            return contract == null
                ? throw new NotFoundException($"Rental contract with id '{id}' was not found.")
                : RentalContractMapper.ToDto(contract);
        }

        public async Task<List<RentalContractResponseDto>> GetAllAsync()
        {
            var contracts = await _rentalContractRepository.GetAllAsync("Owner", "Cargos");

            if (contracts.Count == 0)
            {
                throw new NotFoundException("Rental contracts not found.");
            }

            return contracts.Select(RentalContractMapper.ToDto).ToList();
        }

        public async Task<RentalContractResponseDto> GetByRentalRequestIdAsync(Guid rentalRequestId)
        {
            var contract = await _rentalContractRepository
                .GetByRentalRequestIdAsync(rentalRequestId, "Owner", "Cargos");

            return contract == null
                ? throw new NotFoundException(
                    $"Rental contract for rental request '{rentalRequestId}' was not found.")
                : RentalContractMapper.ToDto(contract);
        }

        public async Task<RentalContractStatusResponseDto> CreateAsync(CreateRentalContractDto dto)
        {
            var offer = await _rentalOfferRepository.GetByIdAsync(dto.OfferId);

            if (offer == null)
            {
                throw new NotFoundException($"Rental offer with id '{dto.OfferId}' was not found.");
            }

            if (offer.Status != RentalOfferStatus.Accepted)
            {
                throw new ValidationException(new
                {
                    OfferId = "A contract can only be generated from an accepted offer."
                });
            }

            var alreadyExists = await _rentalContractRepository
                .ExistsForRentalRequestAsync(offer.RentalRequestId);

            if (alreadyExists)
            {
                throw new ValidationException(new
                {
                    OfferId = "A contract already exists for this rental request."
                });
            }

            var rentalRequest = await _rentalRepository.GetByIdAsync(
                offer.RentalRequestId,
                "Cargos.CargoType"
            );

            if (rentalRequest == null)
            {
                throw new NotFoundException(
                    $"Rental request with id '{offer.RentalRequestId}' was not found.");
            }

            if (rentalRequest.Cargos == null || rentalRequest.Cargos.Count == 0)
            {
                throw new ValidationException(new
                {
                    RentalRequestId = "Cannot generate a contract without cargo details."
                });
            }

            var demurrageRate = dto.DemurrageRate ?? offer.RatePerDay;
            var despatchRate = dto.DemurrageRate ?? (offer.RatePerDay/2);
            var startDate = rentalRequest.StartDate.Date;
            var endDate = startDate.AddDays(rentalRequest.PlanDay);

            var contractNum = await GenerateContractNumAsync(startDate);
            var vessel = await _vesselRepository.GetByIdAsync(rentalRequest.VesselId);

            if (vessel == null)
            {
                throw new NotFoundException($"Vessel with id '{rentalRequest.VesselId}' was not found.");
            }

            if (vessel.Status != VesselStatus.Available)
            {
                throw new ValidationException(new
                {
                    VesselId = "The vessel is no longer available."
                });
            }

            return await _context.ExecuteInTransactionAsync(async () =>
            {
                var contract = new RentalContract
                {
                    Id = Guid.NewGuid(),
                    ContractNum = contractNum,

                    RentalRequestId = offer.RentalRequestId,
                    OwnerId = offer.OwnerId,

                    StartDate = startDate,
                    EndDate = endDate,

                    DemurrageRate = demurrageRate,
                    DespatchRate = despatchRate,

                    AgreedRatePerDay = offer.RatePerDay,
                    AgreedHireAmount = offer.HireAmount,
                    AgreedBunkerAmount = offer.BunkerAmount,
                    AgreedOtherCharges = offer.OtherCharges,
                    AgreedTotalPrice = offer.TotalPrice,

                    Status = RentalContractStatus.Active,

                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                foreach (var cargo in rentalRequest.Cargos)
                {
                    contract.ContractCargos.Add(new ContractCargo
                    {
                        Id = Guid.NewGuid(),
                        ContractId = contract.Id,
                        CargoTypeId = cargo.CargoTypeId,
                        CargoName = cargo.CargoType?.Name ?? "Unknown",
                        Quantity = (double)cargo.Quantity,
                        Unit = cargo.Unit,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }

                await _rentalContractRepository.AddAsync(contract);
                vessel.Status = VesselStatus.Unavailable;
                vessel.UpdateAt = DateTime.UtcNow;
                _vesselRepository.Update(vessel);
                await _rentalContractRepository.SaveChangesAsync();

                return RentalContractMapper.ToStatusDto(contract);
            });
        }
        public async Task<RentalContractStatusResponseDto> CompleteAsync(Guid id)
        {
            var contract = await _rentalContractRepository.GetByIdAsync(id);
            if (contract == null)
                throw new NotFoundException($"Rental contract with id '{id}' was not found.");

            if (contract.Status != RentalContractStatus.Active)
                throw new AppException("Only active contracts can be completed", HttpStatusCode.Conflict, "INVALID_STATUS");

            var rentalRequest = await _rentalRepository.GetByIdAsync(contract.RentalRequestId);
            var vessel = rentalRequest != null
                ? await _vesselRepository.GetByIdAsync(rentalRequest.VesselId)
                : null;

            // TAMBAHAN: jumlahkan seluruh net laytime (demurrage - despatch)
            var laytimeRecords = await _laytimeRecordRepository.GetByContractIdAsync(contract.Id);
            var totalLaytimeAdjustment = laytimeRecords.Sum(x => x.NetLaytimeAmount);

            var finalSettlement = (contract.AgreedTotalPrice ?? 0) + totalLaytimeAdjustment;

            return await _context.ExecuteInTransactionAsync(async () =>
            {
                contract.Status = RentalContractStatus.Complete;
                contract.TotalLaytimeAdjustment = totalLaytimeAdjustment;
                contract.FinalSettlementAmount = finalSettlement;
                contract.CompletedAt = DateTime.UtcNow;
                contract.UpdatedAt = DateTime.UtcNow;
                _rentalContractRepository.Update(contract);

                if (vessel != null)
                {
                    vessel.Status = VesselStatus.Available;
                    vessel.UpdateAt = DateTime.UtcNow;
                    _vesselRepository.Update(vessel);
                }

                await _rentalContractRepository.SaveChangesAsync();

                return RentalContractMapper.ToStatusDto(contract);
            });
        }
        public async Task<RentalContractStatusResponseDto> CancelAsync(Guid id)
        {
            var contract = await _rentalContractRepository.GetByIdAsync(id);

            if (contract == null)
            {
                throw new NotFoundException($"Rental contract with id '{id}' was not found.");
            }

            if (contract.Status != RentalContractStatus.Active)
            {
                throw new AppException(
                    "Only active contracts can be cancelled",
                    HttpStatusCode.Conflict,
                    "INVALID_STATUS");
            }

            contract.Status = RentalContractStatus.Cancelled;
            contract.UpdatedAt = DateTime.UtcNow;

            _rentalContractRepository.Update(contract);
            await _rentalContractRepository.SaveChangesAsync();

            return RentalContractMapper.ToStatusDto(contract);
        }

        private async Task<string> GenerateContractNumAsync(DateTime startDate)
        {
            var prefix = $"TKK-{startDate:yyyyMM}-";
            var count = await _rentalContractRepository.CountByDatePrefixAsync(prefix);
            var sequence = (count + 1).ToString("D4");

            return $"{prefix}{sequence}";
        }

    }
}
