using tongkangku_be.Dtos;
using tongkangku_be.Interfaces;
using tongkangku_be.Mappers;
using tongkangku_be.Models;
using tongkangku_be.Repositories;
using tongkangku_be.Shared;

namespace tongkangku_be.Services
{
    public class RentalService(IRepository<RentalRequest> rentalRepository) : IRentalService
    {
        private readonly IRepository<RentalRequest> _rentalRepository =
        rentalRepository;

        public async Task<RentalResponseDto> GetByIdAsync(Guid id)
        {
            var rental = await _rentalRepository.GetByIdAsync(
                    id,
                    "Vessel",
                    "Charterer"
                );

            return rental == null ? throw new NotFoundException($"Rental request with id '{id}' was not found.") : RentalMapper.ToDto(rental);
        }
    }
}
