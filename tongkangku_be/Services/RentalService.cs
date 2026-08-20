using tongkangku_be.Data;
using tongkangku_be.Dtos;
using tongkangku_be.Interfaces;
using tongkangku_be.Mappers;
using tongkangku_be.Models;
using tongkangku_be.Repositories;
using tongkangku_be.Shared;

namespace tongkangku_be.Services
{
    public class RentalService(IRepository<RentalRequest> rentalRepository, IRepository<Vessel> vesselRepository, IRepository<User> userRepository, ApplicationDbContext context) : IRentalService
    {
        private readonly IRepository<RentalRequest> _rentalRepository =
        rentalRepository;
        private readonly IRepository<Vessel> _vesselRepository =
        vesselRepository;
        private readonly IRepository<User> _userRepository =
        userRepository;
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

        public async Task<RentalResponseDto> CreateAsync(CreateRentalDto dto)
        {
            if(dto.PlanDay <= 0)
            {
                throw new ValidationException(new
                {
                    PlanDay = "Plan day must be greater than 0."
                });
            }

            if(dto.StartDate < DateTime.UtcNow)
            {
                throw new ValidationException(new
                {
                    StartDate = "Start date cannot be in the past."
                });

            }

            var vessel = await _vesselRepository.GetByIdAsync(dto.VesselId);

            if (vessel == null)
            {
                throw new NotFoundException($"Vessel with id '{dto.VesselId}' was not found.");
            }

            var charterer = await _userRepository.GetByIdAsync(dto.ChartererId);

            if (charterer == null)
            {
                throw new NotFoundException($"Charterer request with id '{dto.ChartererId}' was not found.");
            }

            var faktorDurasi = dto.PlanDay switch
            {
                < 7 => 1.3m,
                <= 30 => 1.0m,
                _ => 0.85m,
            };
            var totalEstimatedPrice = vessel.RatePerDay * dto.PlanDay * faktorDurasi;

            return await _context.ExecuteInTransactionAsync(async () =>
            {

            var rental = new RentalRequest
            {
                Id = Guid.NewGuid(),
                VesselId = dto.VesselId,
                ChartererId = dto.ChartererId,
                StartDate = dto.StartDate,
                PlanDay = dto.PlanDay,
                TotalEstimatedPrice = totalEstimatedPrice,
                Status = 0,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };

            await _rentalRepository.AddAsync(rental);
            await _rentalRepository.SaveChangesAsync();

            var createRental = await _rentalRepository.GetByIdAsync(rental.Id, "Vessel", "Charterer");

            if (createRental == null)
            {
                throw new NotFoundException($"Rental request with id '{rental.Id}' was not found.");
            }

            return RentalMapper.ToDto(createRental);

            });
        }
    }
}
