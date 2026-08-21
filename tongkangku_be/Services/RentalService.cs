using Microsoft.EntityFrameworkCore;
using System.Net;
using tongkangku_be.Data;
using tongkangku_be.Dtos.RentalRequest;
using tongkangku_be.Interfaces;
using tongkangku_be.Mappers;
using tongkangku_be.Models;
using tongkangku_be.Models.Enums;
using tongkangku_be.Repositories;
using tongkangku_be.Shared;

namespace tongkangku_be.Services
{
    public class RentalService(IRentalRepository rentalRepository, IRepository<Vessel> vesselRepository, IRepository<User> userRepository, ApplicationDbContext context) : IRentalService
    {
        private readonly IRentalRepository _rentalRepository = rentalRepository;
        private readonly IRepository<Vessel> _vesselRepository = vesselRepository;
        private readonly IRepository<User> _userRepository = userRepository;
        private readonly ApplicationDbContext _context = context;

        public async Task<RentalResponseDto> GetByIdAsync(Guid id)
        {
            var rental = await _rentalRepository.GetByIdAsync(
                    id,
                    "Vessel",
                    "Charterer"
                );

            return rental == null ? throw new NotFoundException($"Rental request with id '{id}' was not found.") : RentalMapper.ToDto(rental);
        }

        public async Task<List<RentalResponseDto>> GetAllAsync()
        {
            var rentals = await _rentalRepository.GetAllAsync(
                "Vessel", "Charterer"
                );

            if (rentals.Count == 0)
            {
                throw new NotFoundException("Rental requests not found.");
            }

            return rentals
                .Select(RentalMapper.ToDto)
                .ToList();
        }

        private async Task<RentalPricingSetting> GetActivePricingSettingAsync()
        {
            var setting = await _context.RentalPricingSettings
                .FirstOrDefaultAsync(x => x.IsActive);

            if (setting == null)
            {
                throw new AppException(
                    "Rental pricing setting is not configured.",
                    HttpStatusCode.InternalServerError,
                    "PRICING_NOT_CONFIGURED");
            }

            return setting;
        }

        private async Task<Dictionary<CostType, decimal>> GetActiveOperationalCostsAsync()
        {
            return await _context.RentalOperationalCosts
                .Where(x => x.IsActive)
                .ToDictionaryAsync(x => x.CostType, x => x.Amount);
        }

        private static decimal GetDurationMultiplier(int planDay, RentalPricingSetting setting)
        {
            if (planDay < setting.ShortDurationMaxDays)
                return setting.ShortDurationMultiplier;

            if (planDay <= setting.MediumDurationMaxDays)
                return setting.MediumDurationMultiplier;

            return setting.LongDurationMultiplier;
        }

        private record PricingBreakdown(
            decimal DurationMultiplier,
            decimal BaseHirePrice,
            decimal AdjustedHirePrice,
            decimal OperationalCost,
            decimal ContingencyCost,
            decimal EstimatedCost
        );

        private static PricingBreakdown CalculatePricing(
            decimal ratePerDay,
            int planDay,
            RentalPricingSetting setting,
            Dictionary<CostType, decimal> operationalCosts)
        {
            var durationMultiplier = GetDurationMultiplier(planDay, setting);

            var baseHirePrice = ratePerDay * planDay;
            var adjustedHirePrice = baseHirePrice * durationMultiplier;

            var operationalCost =
                operationalCosts[CostType.Agency] +
                operationalCosts[CostType.Loading] +
                operationalCosts[CostType.Discharging] +
                operationalCosts[CostType.Other];

            var contingencyCost = operationalCost * setting.ContingencyRate;
            var estimatedCost = adjustedHirePrice + operationalCost + contingencyCost;

            return new PricingBreakdown(
                durationMultiplier, baseHirePrice, adjustedHirePrice,
                operationalCost, contingencyCost, estimatedCost);
        }

        public async Task<RentalStatusResponseDto> CreateAsync(CreateRentalDto dto, Guid chartererId)
        {
            if (dto.PlanDay <= 0)
            {
                throw new ValidationException(new
                {
                    PlanDay = "Plan day must be greater than 0."
                });
            }

            var today = DateTime.UtcNow.Date;

            if (dto.StartDate.Date < today)
            {
                throw new ValidationException(new
                {
                    StartDate = "Start date cannot be in the past."
                });
            }

            var vessel = await _vesselRepository.GetByIdAsync(dto.VesselId);

            if (vessel == null)
            {
                throw new NotFoundException(
                    $"Vessel with id '{dto.VesselId}' was not found."
                );
            }

            if (vessel.Status != VesselStatus.Available)
            {
                throw new ValidationException(new
                {
                    VesselId = "The selected vessel is not available for rental."
                });
            }

            if (vessel.RatePerDay <= 0)
            {
                throw new ValidationException(new
                {
                    VesselId = "Vessel rate per day is not configured."
                });
            }

            var charterer = await _userRepository.GetByIdAsync(chartererId);

            if (charterer == null)
            {
                throw new NotFoundException(
                    $"Charterer with id '{chartererId}' was not found."
                );
            }

            if (charterer.Role != UserRole.Charterer)
            {
                throw new ValidationException(new
                {
                    Charterer = "The current user is not registered as a charterer."
                });
            }

            var loadingPort = await _context.Ports
                .FirstOrDefaultAsync(x => x.Id == dto.LoadingPortId);

            if (loadingPort == null)
            {
                throw new NotFoundException(
                    $"Loading port with id '{dto.LoadingPortId}' was not found."
                );
            }

            var dischargingPort = await _context.Ports
                .FirstOrDefaultAsync(x => x.Id == dto.DischargingPortId);

            if (dischargingPort == null)
            {
                throw new NotFoundException(
                    $"Discharging port with id '{dto.DischargingPortId}' was not found."
                );
            }

            if (dto.LoadingPortId == dto.DischargingPortId)
            {
                throw new ValidationException(new
                {
                    DischargingPortId =
                        "Loading port and discharging port cannot be the same."
                });
            }

            if (dto.Cargos == null || dto.Cargos.Count == 0)
            {
                throw new ValidationException(new
                {
                    Cargos = "At least one cargo is required."
                });
            }

            foreach (var cargo in dto.Cargos)
            {
                if (cargo.Quantity <= 0)
                {
                    throw new ValidationException(new
                    {
                        Cargos = "Cargo quantity must be greater than 0."
                    });
                }

                if (string.IsNullOrWhiteSpace(cargo.Unit))
                {
                    throw new ValidationException(new
                    {
                        Cargos = "Cargo unit is required."
                    });
                }

                var cargoTypeExists = await _context.CargoTypes
                    .AnyAsync(x => x.Id == cargo.CargoTypeId);

                if (!cargoTypeExists)
                {
                    throw new NotFoundException(
                        $"Cargo type with id '{cargo.CargoTypeId}' was not found."
                    );
                }
            }

            var duplicateCargo = dto.Cargos
                .GroupBy(x => x.CargoTypeId)
                .Any(x => x.Count() > 1);

            if (duplicateCargo)
            {
                throw new ValidationException(new
                {
                    Cargos = "The same cargo type cannot be added more than once."
                });
            }

            var startDate = dto.StartDate
             .Date
             .ToUniversalTime();

            var endDate = startDate.AddDays(dto.PlanDay);

            var hasConflict =
                await _rentalRepository.HasActiveRentalConflictAsync(
                    dto.VesselId,
                    startDate,
                    endDate
                );


            if (hasConflict)
            {
                throw new ValidationException(new
                {
                    VesselId = "The vessel is already booked during the requested period."
                });
            }

            var pricingSetting = await GetActivePricingSettingAsync();

            if (pricingSetting.ContingencyRate < 0)
            {
                throw new AppException(
                    "Contingency rate cannot be negative.",
                    HttpStatusCode.InternalServerError,
                    "INVALID_CONTINGENCY_RATE"
                );
            }

            var operationalCosts = await GetActiveOperationalCostsAsync()
                ?? new Dictionary<CostType, decimal>();

            var requiredCostTypes = new[]
            {
                CostType.Agency,
                CostType.Loading,
                CostType.Discharging,
                CostType.Other
            };

            var missingCostTypes = requiredCostTypes
                .Where(x => !operationalCosts.ContainsKey(x))
                .ToList();

            if (missingCostTypes.Count > 0)
            {
                throw new AppException(
                    $"Operational cost rate not configured for: {string.Join(", ", missingCostTypes)}",
                    HttpStatusCode.InternalServerError,
                    "COST_RATE_NOT_CONFIGURED"
                );
            }

            var breakdown = CalculatePricing(vessel.RatePerDay, dto.PlanDay, pricingSetting, operationalCosts);

            if (breakdown.DurationMultiplier <= 0)
            {
                throw new AppException(
                    "Invalid duration multiplier.",
                    HttpStatusCode.InternalServerError,
                    "INVALID_DURATION_MULTIPLIER"
                );
            }

            var agencyCost = operationalCosts[CostType.Agency];
            var loadingCost = operationalCosts[CostType.Loading];
            var dischargingCost = operationalCosts[CostType.Discharging];
            var otherOperationalCost = operationalCosts[CostType.Other];

            return await _context.ExecuteInTransactionAsync(async () =>
            {
                var rental = new RentalRequest
                {
                    Id = Guid.NewGuid(),
                    VesselId = dto.VesselId,
                    ChartererId = chartererId,
                    CharterType = dto.CharterType,
                    LoadingPortId = dto.LoadingPortId,
                    DischargingPortId = dto.DischargingPortId,
                    StartDate = startDate,
                    PlanDay = dto.PlanDay,
                    BaseHirePrice = breakdown.BaseHirePrice,
                    DurationMultiplier = breakdown.DurationMultiplier,
                    EstimatedCost = breakdown.EstimatedCost,
                    TargetMargin = 0m,
                    TotalEstimatedPrice = breakdown.EstimatedCost,
                    Status = RentalRequestStatus.Pending,
                    Notes = dto.Notes,
                    CreatedAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow,
                    Cargos = new List<RentalRequestCargo>()
                };

                foreach (var cargoDto in dto.Cargos)
                {
                    rental.Cargos.Add(new RentalRequestCargo
                    {
                        Id = Guid.NewGuid(),
                        RentalRequestId = rental.Id,
                        CargoTypeId = cargoDto.CargoTypeId,
                        Quantity = cargoDto.Quantity,
                        Unit = cargoDto.Unit,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }

                var now = DateTime.UtcNow;

                rental.CostItems = new List<RentalCostItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        RentalRequestId = rental.Id,
                        CostType = CostType.Agency,
                        Bearer = CostBearer.Charterer,
                        Amount = agencyCost,
                        Notes = "Agency cost",
                        CreatedAt = now,
                        UpdatedAt = now
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        RentalRequestId = rental.Id,
                        CostType = CostType.Loading,
                        Bearer = CostBearer.Charterer,
                        Amount = loadingCost,
                        Notes = "Loading operational cost",
                        CreatedAt = now,
                        UpdatedAt = now
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        RentalRequestId = rental.Id,
                        CostType = CostType.Discharging,
                        Bearer = CostBearer.Charterer,
                        Amount = dischargingCost,
                        Notes = "Discharging operational cost",
                        CreatedAt = now,
                        UpdatedAt = now
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        RentalRequestId = rental.Id,
                        CostType = CostType.Other,
                        Bearer = CostBearer.Charterer,
                        Amount = otherOperationalCost,
                        Notes = "Other operational cost",
                        CreatedAt = now,
                        UpdatedAt = now
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        RentalRequestId = rental.Id,
                        CostType = CostType.Contingency,
                        Bearer = CostBearer.Charterer,
                        Amount = breakdown.ContingencyCost,
                        Notes = $"{pricingSetting.ContingencyRate:P0} operational cost contingency",
                        CreatedAt = now,
                        UpdatedAt = now
                    }
                };

                await _rentalRepository.AddAsync(rental);
                await _rentalRepository.SaveChangesAsync();

                var createdRental = await _rentalRepository.GetByIdAsync(
                    rental.Id,
                    "Vessel",
                    "Charterer",
                    "LoadingPort",
                    "DischargingPort",
                    "Cargos",
                    "CostItems"
                );

                if (createdRental == null)
                {
                    throw new NotFoundException(
                        $"Rental request with id '{rental.Id}' was not found."
                    );
                }

                return RentalMapper.ToStatusDto(createdRental);
            });
        }

        public async Task<RentalStatusResponseDto> UpdateAsync(Guid id, UpdateRentalDto dto)
        {
            if (dto.PlanDay <= 0)
            {
                throw new ValidationException(new { PlanDay = "Plan day must be greater than 0." });
            }

            var rental = await _rentalRepository.GetByIdAsync(id);
            if (rental is null)
                throw new NotFoundException($"Rental request with id '{id}' was not found.");

            var vessel = await _vesselRepository.GetByIdAsync(dto.VesselId);
            if (vessel is null)
                throw new NotFoundException($"Vessel with id '{dto.VesselId}' was not found.");

            var pricingSetting = await GetActivePricingSettingAsync();
            var operationalCosts = await GetActiveOperationalCostsAsync();

            var breakdown = CalculatePricing(vessel.RatePerDay, dto.PlanDay, pricingSetting, operationalCosts);

            await _context.ExecuteInTransactionAsync(async () =>
            {
                rental.VesselId = dto.VesselId;
                rental.StartDate = dto.StartDate;
                rental.PlanDay = dto.PlanDay;
                rental.Notes = dto.Notes;
                rental.BaseHirePrice = breakdown.BaseHirePrice;
                rental.DurationMultiplier = breakdown.DurationMultiplier;
                rental.EstimatedCost = breakdown.EstimatedCost;
                rental.TotalEstimatedPrice = breakdown.EstimatedCost;
                rental.UpdateAt = DateTime.UtcNow;

                _rentalRepository.Update(rental);
                await _rentalRepository.SaveChangesAsync();
            });

            return RentalMapper.ToStatusDto(rental);
        }

        public async Task<RentalStatusResponseDto> CancelAsync(Guid id, Guid chartererId)
        {
            var rental = await _rentalRepository.GetByIdAsync(id);

            if (rental == null)
            {
                throw new NotFoundException($"Rental request with id '{id}' was not found.");
            }

            if (rental.ChartererId != chartererId)
            {
                throw new ValidationException(new
                {
                    Rental = "You can only cancel your own rental request."
                });
            }

            if (rental.Status != RentalRequestStatus.Pending)
            {
                throw new AppException(
                    "Only pending rental requests can be cancelled",
                    HttpStatusCode.Conflict,
                    "INVALID_STATUS");
            }

            rental.Status = RentalRequestStatus.Cancelled;
            rental.UpdateAt = DateTime.UtcNow;

            _rentalRepository.Update(rental);
            await _rentalRepository.SaveChangesAsync();

            return RentalMapper.ToStatusDto(rental);
        }

        public async Task<RentalStatusResponseDto> ApproveAsync(Guid id)
        {
            var rental = await _rentalRepository.GetByIdAsync(id);
            if (rental == null)
            {
                throw new NotFoundException(
                   $"Rental request with id '{id}' was not found."
               );
            }
            if (rental.Status != (int)RentalRequestStatus.Pending)
            {
                throw new AppException("Only pending rental requests can be approved", HttpStatusCode.Conflict, "INVALID_STATUS");
            }

            rental.Status = (RentalRequestStatus)(int)RentalRequestStatus.Approved;
            rental.UpdateAt = DateTime.UtcNow;

            _rentalRepository.Update(rental);

            await _rentalRepository.SaveChangesAsync();

            return RentalMapper.ToStatusDto(rental);
        }

        public async Task<RentalEstimateResponseDto> EstimateAsync(EstimateRentalDto dto)
        {
            if (dto.PlanDay <= 0)
            {
                throw new ValidationException(new { PlanDay = "Plan day must be greater than 0." });
            }

            if (dto.StartDate.Date < DateTime.UtcNow.Date)
            {
                throw new ValidationException(new { StartDate = "Start date cannot be in the past." });
            }

            var vessel = await _vesselRepository.GetByIdAsync(dto.VesselId);

            if (vessel == null)
            {
                throw new NotFoundException($"Vessel with id '{dto.VesselId}' was not found.");
            }

            var pricingSetting = await GetActivePricingSettingAsync();
            var operationalCosts = await GetActiveOperationalCostsAsync();

            var breakdown = CalculatePricing(vessel.RatePerDay, dto.PlanDay, pricingSetting, operationalCosts);

            var taxRate = 0.012m;
            var taxAmount = breakdown.EstimatedCost * taxRate;
            var grandTotal = breakdown.EstimatedCost + taxAmount;

            return new RentalEstimateResponseDto
            {
                VesselId = vessel.Id,
                VesselName = vessel.Name ?? string.Empty,
                IsVesselAvailable = vessel.Status == VesselStatus.Available,

                RatePerDay = vessel.RatePerDay,
                PlanDay = dto.PlanDay,
                DurationMultiplier = breakdown.DurationMultiplier,

                BaseHirePrice = breakdown.BaseHirePrice,
                AdjustedHirePrice = breakdown.AdjustedHirePrice,
                OperationalCost = breakdown.OperationalCost,
                ContingencyCost = breakdown.ContingencyCost,
                EstimatedCost = breakdown.EstimatedCost,
                TotalEstimatedPrice = breakdown.EstimatedCost,

                TaxRate = taxRate,
                TaxAmount = taxAmount,

                GrandTotal = grandTotal
            };
        }

        public async Task<RentalStatusResponseDto> RejectAsync(Guid id, RejectRentalDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Reason))
            {
                throw new ValidationException(new
                {
                    Reason = "Rejection reason is required"
                });
            }

            var rental = await _rentalRepository.GetByIdAsync(id);
            if (rental == null)
            {
                throw new NotFoundException(
                   $"Rental request with id '{id}' was not found."
               );
            }

            if (rental.Status != (int)RentalRequestStatus.Pending)
            {
                throw new AppException("Only pending rental requests can be approved", HttpStatusCode.Conflict, "INVALID_STATUS");
            }

            rental.Status = (RentalRequestStatus)(int)RentalRequestStatus.Rejected;
            rental.UpdateAt = DateTime.UtcNow;

            rental.RejectionReason = dto.Reason;
            _rentalRepository.Update(rental);
            await _rentalRepository.SaveChangesAsync();
            return RentalMapper.ToStatusDto(rental);
        }
    }
}