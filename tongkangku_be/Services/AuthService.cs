using BCrypt.Net;
using tongkangku_be.Dtos;
using tongkangku_be.Interfaces;
using tongkangku_be.Models;
using tongkangku_be.Repositories;

namespace tongkangku_be.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IRepository<User> userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }


        public async Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto request)
        {

            if (request == null)
            {
                return null;
            }

            var password = BCrypt.Net.BCrypt.HashPassword(request.password);

            var newUser = new User
            {

                Name = request.name,
                Email = request.email,
                Password = password,
                Role = (UserRole)request.role

            };

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            var ResponseDto = new RegisterResponseDto
            {
                id = newUser.Id,
                name = newUser.Name,
                email = newUser.Email,
                role = newUser.Role.ToString()
            };
            return ResponseDto;

        }
    }
}
 
