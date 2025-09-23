using Application.Interfaces;
using Common;
using Domain.Entities;
using Domain.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
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

        public async Task<ResultWrapper> LoginAsync(LoginRequestDTO loginRequestParams)
        {
            ResultWrapper rw = new ResultWrapper();
            string token = string.Empty;
            User user = new User();

            try
            {
                // TODO: Replace with real user validation
                rw = await _userService.GetByEmailAsync(loginRequestParams.Email);
                if (!rw.Success)
                {
                    throw new Exception(rw.Message);
                }
                user = (User)rw.Data;

                if (user != null)
                {
                    token = _jwtService.GenerateToken(user);
                }

                // TODO: Validate password
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Unable to login. Invalid token.");
                }

                rw.Data = new { Token = token, User = user };
                rw.Success = true;
            }
            catch (Exception ex)
            {
                rw.Data = new { };
                rw.Success = false;
                rw.Message = ex.Message;
            }

            return rw;
        }

        public async Task<ResultWrapper> RegisterAsync(RegisterRequestDto dto)
        {
            ResultWrapper rw = new ResultWrapper();

            try
            {
                // TODO: Implement registration logic
                var user = new User { Id = Guid.NewGuid(), Name = dto.Name, Email = dto.Email };
                // TODO: Save user
                var token = _jwtService.GenerateToken(user);
                rw.Data = new { Token = token, User = user };
                rw.Success = true;
            }
            catch (Exception ex)
            {
                rw.Data = new { };
                rw.Success = false;
                rw.Message = ex.Message;
            }

            return rw;
        }

        public async Task<ResultWrapper> GetProfileAsync(string email)
        {
            ResultWrapper rw = new ResultWrapper();

            try
            {
                rw.Data = await _userService.GetByEmailAsync(email);
                rw.Success = true;
            }
            catch (Exception ex)
            {
                rw.Data = new { };
                rw.Success = false;
                rw.Message = ex.Message;
            }

            return rw;
        }

        public Task LogoutAsync(string token)
        {
            // JWT logout is stateless; implement blacklist if needed
            return Task.CompletedTask;
        }
    }
}