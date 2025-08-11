using Application.DTOs;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;

        public AuthService(IUserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            // TODO: Replace with real user validation
            var user = await _userService.GetByEmailAsync(dto.Email);
            if (user == null) return null;
            // TODO: Validate password
            var token = _jwtService.GenerateToken(user);
            return new LoginResponseDto { Token = token, User = user };
        }

        public async Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto dto)
        {
            // TODO: Implement registration logic
            var user = new UserDto { Id = Guid.NewGuid(), Name = dto.Name, Email = dto.Email };
            // TODO: Save user
            var token = _jwtService.GenerateToken(user);
            return new RegisterResponseDto { Token = token, User = user };
        }

        public async Task<UserDto?> GetProfileAsync(string email)
        {
            return await _userService.GetByEmailAsync(email);
        }

        public Task LogoutAsync(string token)
        {
            // JWT logout is stateless; implement blacklist if needed
            return Task.CompletedTask;
        }
    }
}