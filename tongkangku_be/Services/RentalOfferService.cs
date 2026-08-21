using System.Net;
using tongkangku_be.Data;
using tongkangku_be.Dtos.RentalOffer;
using tongkangku_be.Interfaces;
using tongkangku_be.Mappers;
using tongkangku_be.Models;
using tongkangku_be.Models.Enums;
using tongkangku_be.Repositories;
using tongkangku_be.Shared;

namespace tongkangku_be.Services
{
    public class RentalOfferService(
        IRentalOfferRepository rentalOfferRepository,
        IRentalRepository rentalRepository,
        IRepository<Vessel> vesselRepository,
        IRepository<User> userRepository,
        IRentalContractService rentalContractService,
        ApplicationDbContext context) : IRentalOfferService
    {
        private readonly IRentalOfferRepository _rentalOfferRepository =
            rentalOfferRepository;

        private readonly IRentalRepository _rentalRepository =
            rentalRepository;

        private readonly IRepository<Vessel> _vesselRepository =
            vesselRepository;

        private readonly IRepository<User> _userRepository =
            userRepository;

        private readonly IRentalContractService _rentalContractService =
            rentalContractService;

        private readonly ApplicationDbContext _context =
            context;

        public async Task<RentalOfferResponseDto> GetByIdAsync(Guid id)
        {
            var offer =
                await _rentalOfferRepository.GetByIdAsync(id);

            return offer == null
                ? throw new NotFoundException(
                    $"Rental offer with id '{id}' was not found.")
                : RentalOfferMapper.ToDto(offer);
        }

        public async Task<List<RentalOfferResponseDto>> GetAllAsync()
        {
            var offers =
                await _rentalOfferRepository.GetAllAsync(
                    "RentalRequest",
                    "Owner"
                );

            if (offers.Count == 0)
            {
                throw new NotFoundException(
                    "Rental offers not found.");
            }

            return offers
                .Select(RentalOfferMapper.ToDto)
                .ToList();
        }

        public async Task<RentalOfferStatusResponseDto> CreateAsync(
            CreateRentalOfferDto dto,
            Guid ownerId)
        {
            var rentalRequest =
                await _rentalRepository.GetByIdAsync(
                    dto.RentalRequestId
                );

            if (rentalRequest == null)
            {
                throw new NotFoundException(
                    $"Rental request with id '{dto.RentalRequestId}' was not found.");
            }

            if (rentalRequest.Status != RentalRequestStatus.Offered)
            {
                throw new ValidationException(new
                {
                    RentalRequestId =
                        "Offers can only be submitted for rental requests with Offered status."
                });
            }

            var owner =
                await _userRepository.GetByIdAsync(ownerId);

            if (owner == null)
            {
                throw new NotFoundException(
                    $"Owner with id '{ownerId}' was not found.");
            }

            if (owner.Role != UserRole.Owner)
            {
                throw new ValidationException(new
                {
                    Owner =
                        "The current user is not registered as an owner."
                });
            }

            var vessel =
                await _vesselRepository.GetByIdAsync(
                    rentalRequest.VesselId
                );

            if (vessel == null)
            {
                throw new NotFoundException(
                    $"Vessel with id '{rentalRequest.VesselId}' was not found.");
            }

            if (vessel.OwnerId != ownerId)
            {
                throw new ValidationException(new
                {
                    Owner =
                        "You can only submit an offer for a vessel you own."
                });
            }

            if (vessel.Status != VesselStatus.Available)
            {
                throw new ValidationException(new
                {
                    VesselId =
                        "The vessel is no longer available."
                });
            }

            if (dto.RatePerDay <= 0)
            {
                throw new ValidationException(new
                {
                    RatePerDay =
                        "Rate per day must be greater than 0."
                });
            }

            if (dto.BunkerAmount < 0)
            {
                throw new ValidationException(new
                {
                    BunkerAmount =
                        "Bunker amount cannot be negative."
                });
            }

            if (dto.OtherCharges < 0)
            {
                throw new ValidationException(new
                {
                    OtherCharges =
                        "Other charges cannot be negative."
                });
            }

            if (dto.ValidUntil.Date < DateTime.UtcNow.Date)
            {
                throw new ValidationException(new
                {
                    ValidUntil =
                        "Valid until date cannot be in the past."
                });
            }

            var hasActiveOffer =
                await _rentalOfferRepository
                    .HasActiveOfferAsync(
                        dto.RentalRequestId,
                        ownerId
                    );

            if (hasActiveOffer)
            {
                throw new ValidationException(new
                {
                    RentalRequestId =
                        "You already have a pending offer for this rental request."
                });
            }

            var hireAmount =
                dto.RatePerDay *
                rentalRequest.PlanDay;

            var totalPrice =
                hireAmount +
                dto.BunkerAmount +
                dto.OtherCharges;

            return await _context.ExecuteInTransactionAsync(
                async () =>
                {
                    var offer = new RentalOffer
                    {
                        Id = Guid.NewGuid(),

                        RentalRequestId =
                            dto.RentalRequestId,

                        OwnerId =
                            ownerId,

                        RatePerDay =
                            dto.RatePerDay,

                        ValidUntil =
                            dto.ValidUntil,

                        HireAmount =
                            hireAmount,

                        TotalPrice =
                            totalPrice,

                        BunkerAmount =
                            dto.BunkerAmount,

                        OtherCharges =
                            dto.OtherCharges,

                        Status =
                            RentalOfferStatus.Pending,

                        Notes =
                            dto.Notes,

                        CreatedAt =
                            DateTime.UtcNow,

                        UpdatedAt =
                            DateTime.UtcNow
                    };

                    await _rentalOfferRepository
                        .AddAsync(offer);

                    await _rentalOfferRepository
                        .SaveChangesAsync();

                    return RentalOfferMapper
                        .ToStatusDto(offer);
                }
            );
        }

        public async Task<List<RentalOfferResponseDto>>
            GetByRentalRequestIdAsync(Guid rentalRequestId)
        {
            var offers =
                await _rentalOfferRepository
                    .GetByRentalRequestIdAsync(
                        rentalRequestId
                    );

            return offers
                .Select(RentalOfferMapper.ToDto)
                .ToList();
        }

        public async Task<RentalOfferStatusResponseDto> UpdateAsync(
            Guid id,
            UpdateRentalOfferDto dto)
        {
            var offer =
                await _rentalOfferRepository.GetByIdAsync(
                    id,
                    "RentalRequest"
                );

            if (offer == null)
            {
                throw new NotFoundException(
                    $"Rental offer with id '{id}' was not found.");
            }

            if (offer.Status != RentalOfferStatus.Pending)
            {
                throw new AppException(
                    "Only pending offers can be updated",
                    HttpStatusCode.Conflict,
                    "INVALID_STATUS"
                );
            }

            if (dto.RatePerDay <= 0)
            {
                throw new ValidationException(new
                {
                    RatePerDay =
                        "Rate per day must be greater than 0."
                });
            }

            if (dto.BunkerAmount < 0)
            {
                throw new ValidationException(new
                {
                    BunkerAmount =
                        "Bunker amount cannot be negative."
                });
            }

            if (dto.OtherCharges < 0)
            {
                throw new ValidationException(new
                {
                    OtherCharges =
                        "Other charges cannot be negative."
                });
            }

            if (dto.ValidUntil.Date < DateTime.UtcNow.Date)
            {
                throw new ValidationException(new
                {
                    ValidUntil =
                        "Valid until date cannot be in the past."
                });
            }

            var hireAmount =
                dto.RatePerDay *
                offer.RentalRequest.PlanDay;

            var totalPrice =
                hireAmount +
                dto.BunkerAmount +
                dto.OtherCharges;

            return await _context.ExecuteInTransactionAsync(
                async () =>
                {
                    offer.RatePerDay =
                        dto.RatePerDay;

                    offer.HireAmount =
                        hireAmount;

                    offer.BunkerAmount =
                        dto.BunkerAmount;

                    offer.OtherCharges =
                        dto.OtherCharges;

                    offer.TotalPrice =
                        totalPrice;

                    offer.ValidUntil =
                        dto.ValidUntil;

                    offer.Notes =
                        dto.Notes;

                    offer.UpdatedAt =
                        DateTime.UtcNow;

                    _rentalOfferRepository.Update(offer);

                    await _rentalOfferRepository
                        .SaveChangesAsync();

                    return RentalOfferMapper
                        .ToStatusDto(offer);
                }
            );
        }

        public async Task DeleteAsync(Guid id)
        {
            var offer =
                await _rentalOfferRepository.GetByIdAsync(id);

            if (offer == null)
            {
                throw new NotFoundException(
                    $"Rental offer with id '{id}' was not found.");
            }

            if (offer.Status != RentalOfferStatus.Pending)
            {
                throw new AppException(
                    "Only pending offers can be deleted",
                    HttpStatusCode.Conflict,
                    "INVALID_STATUS"
                );
            }

            _rentalOfferRepository.Delete(offer);

            await _rentalOfferRepository.SaveChangesAsync();
        }

        public async Task<RentalOfferStatusResponseDto> WithdrawAsync(
            Guid id)
        {
            var offer =
                await _rentalOfferRepository.GetByIdAsync(id);

            if (offer == null)
            {
                throw new NotFoundException(
                    $"Rental offer with id '{id}' was not found.");
            }

            if (offer.Status != RentalOfferStatus.Pending)
            {
                throw new AppException(
                    "Only pending offers can be withdrawn",
                    HttpStatusCode.Conflict,
                    "INVALID_STATUS"
                );
            }

            offer.Status =
                RentalOfferStatus.Withdrawn;

            offer.UpdatedAt =
                DateTime.UtcNow;

            _rentalOfferRepository.Update(offer);

            await _rentalOfferRepository
                .SaveChangesAsync();

            return RentalOfferMapper
                .ToStatusDto(offer);
        }

        public async Task<RentalOfferStatusResponseDto> AcceptAsync(
            Guid id)
        {
            var offer =
                await _rentalOfferRepository.GetByIdAsync(id);

            if (offer == null)
            {
                throw new NotFoundException(
                    $"Rental offer with id '{id}' was not found.");
            }

            if (offer.Status != RentalOfferStatus.Pending)
            {
                throw new AppException(
                    "Only pending offers can be accepted",
                    HttpStatusCode.Conflict,
                    "INVALID_STATUS"
                );
            }

            if (offer.ValidUntil.Date < DateTime.UtcNow.Date)
            {
                throw new ValidationException(new
                {
                    ValidUntil =
                        "This offer has expired and can no longer be accepted."
                });
            }

            var rentalRequest =
                await _rentalRepository.GetByIdAsync(
                    offer.RentalRequestId
                );

            if (rentalRequest == null)
            {
                throw new NotFoundException(
                    $"Rental request with id '{offer.RentalRequestId}' was not found.");
            }

            if (rentalRequest.Status != RentalRequestStatus.Offered)
            {
                throw new AppException(
                    "This rental request is no longer available for offer acceptance.",
                    HttpStatusCode.Conflict,
                    "INVALID_RENTAL_REQUEST_STATUS"
                );
            }

            return await _context.ExecuteInTransactionAsync(
                async () =>
                {
                    offer.Status =
                        RentalOfferStatus.Accepted;

                    offer.UpdatedAt =
                        DateTime.UtcNow;

                    _rentalOfferRepository.Update(offer);

                    var otherOffers =
                        await _rentalOfferRepository
                            .GetByRentalRequestIdAsync(
                                offer.RentalRequestId
                            );

                    foreach (
                        var otherOffer in
                        otherOffers.Where(x =>
                            x.Id != offer.Id &&
                            x.Status ==
                                RentalOfferStatus.Pending))
                    {
                        otherOffer.Status =
                            RentalOfferStatus.Rejected;

                        otherOffer.RejectionReason =
                            "Another offer was accepted for this rental request.";

                        otherOffer.UpdatedAt =
                            DateTime.UtcNow;

                        _rentalOfferRepository
                            .Update(otherOffer);
                    }

                    await _rentalOfferRepository
                        .SaveChangesAsync();

                    await _rentalContractService
                        .CreateFromAcceptedOfferAsync(
                            offer.Id
                        );

                    return RentalOfferMapper
                        .ToStatusDto(offer);
                }
            );
        }

        public async Task<RentalOfferStatusResponseDto> RejectAsync(
            Guid id,
            RejectRentalOfferDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Reason))
            {
                throw new ValidationException(new
                {
                    Reason =
                        "Rejection reason is required."
                });
            }

            var offer =
                await _rentalOfferRepository
                    .GetByIdAsync(id);

            if (offer == null)
            {
                throw new NotFoundException(
                    $"Rental offer with id '{id}' was not found.");
            }

            if (offer.Status != RentalOfferStatus.Pending)
            {
                throw new AppException(
                    "Only pending offers can be rejected",
                    HttpStatusCode.Conflict,
                    "INVALID_STATUS"
                );
            }

            offer.Status =
                RentalOfferStatus.Rejected;

            offer.RejectionReason =
                dto.Reason;

            offer.UpdatedAt =
                DateTime.UtcNow;

            _rentalOfferRepository.Update(offer);

            await _rentalOfferRepository
                .SaveChangesAsync();

            return RentalOfferMapper
                .ToStatusDto(offer);
        }
    }
}
