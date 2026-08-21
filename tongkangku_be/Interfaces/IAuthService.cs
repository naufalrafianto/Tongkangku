using System.Threading.Tasks;
using tongkangku_be.Dtos.AuthRequest;

namespace tongkangku_be.Interfaces
{
    public interface IAuthService
    {
       
        Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto request);
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
        Task<CurrentUserResponseDto> CurrentUserAsync();
    }
}
