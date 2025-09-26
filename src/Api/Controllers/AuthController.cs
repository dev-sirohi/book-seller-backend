using Application.Interfaces;
using BSB.src.Common;
using BSB.src.Common.Database.DBInterfaces;
using BSB.src.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IDBConnection _cn;
        public AuthController(
            IConfiguration configuration,
            IDBConnection connection,
            IAuthService authService)
        {
            _configuration = configuration;
            _cn = connection;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestParams)
        {
            ResultWrapper rw = new ResultWrapper();
            LoginReturnDto loginReturnDTO = new LoginReturnDto();

            try
            {
                using (IDBTransaction tx = _cn.BeginTransaction())
                {
                    if (string.IsNullOrEmpty(loginRequestParams.Email)
                    || string.IsNullOrEmpty(loginRequestParams.Password))
                    {
                        throw new Exception("Invalid email or password");
                    }

                    rw = await _authService.LoginAsync(loginRequestParams, _cn, tx);
                    if (!rw.Success)
                    {
                        throw new Exception(rw.Message);
                    }
                    loginReturnDTO = (LoginReturnDto)(rw.Data);

                    await tx.CommitAsync();
                }

                rw.Data = loginReturnDTO;
                rw.Success = true;
                rw.Message = "Login successful";
            }
            catch (Exception ex)
            {
                rw.Success = false;
                rw.Message = ex.Message;
                return BadRequest(rw);
            }

            return Ok(rw);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            ResultWrapper rw = new ResultWrapper();
            try
            {
                rw.Data = (RegisterResponseDto?)(await _authService.RegisterAsync(dto));
                if (rw.Data == null)
                {
                    rw.Success = false;
                    rw.Message = "Invalid email or password";
                    return Unauthorized(rw);
                }
            }
            catch (Exception ex)
            {
                rw.Success = false;
                rw.Message = ex.Message;
                return BadRequest(rw);
            }

            return Ok(rw);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            ResultWrapper rw = new ResultWrapper();
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(email)) return Unauthorized();
                var user = await _authService.GetProfileAsync(email);
                if (user == null)
                {
                    rw.Success = false;
                    rw.Message = "Invalid email or password.";
                    return Unauthorized(rw);
                }
                rw.Data = user;
            }
            catch (Exception ex)
            {
                rw.Success = false;
                rw.Message = ex.Message;
                return BadRequest(rw);
            }
            // Get email from JWT claims
            
            return Ok(rw);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            ResultWrapper rw = new ResultWrapper();
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                await _authService.LogoutAsync(token);
            }
            catch (Exception ex)
            {
                rw.Success = false;
                rw.Message = ex.Message;
                return BadRequest(rw);
            }
            // For demo, get token from header
            
            return Ok(rw);
        }
    }
}