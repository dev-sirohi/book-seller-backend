using Application.Interfaces;
using BSB.src.Domain.Entities;
using BSB.src.Domain.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using BSB.src.Common;
using Microsoft.AspNetCore.Identity.Data;
using BSB.src.Common.Database.DBInterfaces;

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

        public async Task<ResultWrapper> LoginAsync(LoginRequestDto loginRequestParams, IDBConnection cn, IDBTransaction tx)
        {
            ResultWrapper rw = new ResultWrapper();
            string _token = string.Empty;
            User user = new User();

            try
            {
                rw = await _userService.GetByEmailAsync(loginRequestParams.Email, cn, tx);
                if (!rw.Success)
                {
                    throw new Exception(rw.Message);
                }
                user = (User)rw.Data;

                rw = await _userService.VerifyUserPassword(user.UserId, loginRequestParams.Password, cn, tx);
                if (!rw.Success)
                {
                    throw new Exception(rw.Message);
                }

                _token = _jwtService.GenerateToken(user);

                if (string.IsNullOrEmpty(_token))
                {
                    throw new Exception("Unable to login. Invalid token.");
                }

                rw.Data = new { Token = _token, User = user };
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

        public async Task<ResultWrapper> RegisterAsync(RegisterRequestDto dto, IDBConnection cn, IDBTransaction tx)
        {
            ResultWrapper rw = new ResultWrapper();

            try
            {
                var user = new User { UserId = Guid.NewGuid(), Name = Utils.ResolveUserName(dto.FirstName, dto.LastName), Email = dto.Email };

                // Save User and Password

                LoginRequestDto loginRequestDto = new LoginRequestDto()
                {
                    Email = dto.Email,
                    Password = dto.Password
                };

                rw = await LoginAsync(loginRequestDto, cn, tx);
                if (!rw.Success)
                {
                    throw new Exception(rw.Message);
                }

                string token = ((dynamic)rw.Data).Token;
                user = ((dynamic)rw.Data).User;

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

        public async Task<ResultWrapper> GetProfileAsync(string email, IDBConnection cn, IDBTransaction tx)
        {
            ResultWrapper rw = new ResultWrapper();

            try
            {
                rw.Data = await _userService.GetByEmailAsync(email, cn, tx);
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

        public Task LogoutAsync(string token, IDBConnection cn, IDBTransaction tx)
        {
            // JWT logout is stateless; implement blacklist if needed
            return Task.CompletedTask;
        }
    }
}