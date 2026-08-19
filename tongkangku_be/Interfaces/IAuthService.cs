using System.Threading.Tasks;
using tongkangku_be.Dtos;

namespace tongkangku_be.Interfaces
{
    public interface IAuthService
    {
       
        Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto request);
    }
}
