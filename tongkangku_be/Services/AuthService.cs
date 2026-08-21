using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tongkangku_be.Dtos.AuthRequest;
using tongkangku_be.Interfaces;
using tongkangku_be.Models;
using tongkangku_be.Repositories;
using System.Security.Claims;
namespace tongkangku_be.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthService(IRepository<User> userRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
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
        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            List<User> users = await _userRepository.GetAllAsync();
            User? user = users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                return null;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isPasswordValid)
            {
                return null;
            }

            Claim[] claims = new Claim[]
            {
                new Claim("id",user.Id.ToString()),
                new Claim("email",user.Email),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    double.Parse(_configuration["Jwt:ExpireMinutes"]!)
                    ),
                signingCredentials: creds
                );

            return new LoginResponseDto
            {
                status = "success",
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                User = new UserSummary
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role.ToString()
                }
            };


        }

        public async Task<CurrentUserResponseDto> CurrentUserAsync()
        {
            var userIdClaim = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                              ?? _contextAccessor.HttpContext?.User?.FindFirst("id");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("Token tidak valid atau belum login!");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User tidak ditemukan!");
            }

            return new CurrentUserResponseDto
            {
                id = user.Id,
                email = user.Email,
                name = user.Name
            };
        }


    }
}
 
