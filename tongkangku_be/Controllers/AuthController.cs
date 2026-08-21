    using Microsoft.AspNetCore.Mvc;
    using tongkangku_be.Dtos.AuthRequest;
    using tongkangku_be.Interfaces;
using tongkangku_be.Shared;

    namespace tongkangku_be.Controllers
    {
        [ApiController]
        [Route("/api/auth")]
        public class AuthController : ControllerBase
        {
            private readonly IAuthService _authService;
            public AuthController(IAuthService authService)
            {
                _authService = authService;

            }
            [HttpPost("register")]
            public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<RegisterResponseDto>.ErrorResult(
                    message: "Seluruh kolom wajib diisi dengan benar",
                    errorCode: "INVALID_PAYLOAD"
                ));
            }

            var result = await _authService.RegisterAsync(request);
            if (result == null)
            {
                return BadRequest(ApiResponse<RegisterResponseDto>.ErrorResult(
                    message: "Gagal me-registrasi user",
                    errorCode: "REGISTRATION_FAILED"
                ));
            }

            return Ok(ApiResponse<RegisterResponseDto>.SuccessResult(
                data: result,
                message: "Registrasi berhasil"
            ));
        }

        [HttpPost("login")]
            public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (result == null)
            {
                return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResult(
                    message: "Email atau password salah",
                    errorCode: "INVALID_CREDENTIALS"
                ));
            }

            return Ok(ApiResponse<LoginResponseDto>.SuccessResult(
                data: result,
                message: "Login berhasil"
            ));
        }
        [HttpGet("me")]
            public async Task<IActionResult> CurrentUserAsync()
        {
            try
            {
                var result = await _authService.CurrentUserAsync();
                return Ok(ApiResponse<CurrentUserResponseDto>.SuccessResult(
                    data: result,
                    message: "Data user berhasil diambil"
                ));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<CurrentUserResponseDto>.ErrorResult(
                    message: ex.Message,
                    errorCode: "UNAUTHORIZED"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<CurrentUserResponseDto>.ErrorResult(
                    message: ex.Message,
                    errorCode: "USER_NOT_FOUND"
                ));
            }
        }
    }
    }
