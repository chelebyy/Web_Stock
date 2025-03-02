using Stock.Application.Models.DTOs;
using System.Threading.Tasks;

namespace Stock.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        string GenerateJwtToken(UserDto user);
    }
} 