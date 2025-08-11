using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Common;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            ResultWrapper rw = new ResultWrapper();
            try
            {
                rw.Data = (LoginResponseDto?)(await _authService.LoginAsync(dto));
                if (rw.Data == null)
                {
                    rw.Success = false;
                    rw.Message = "Invalid email or password.";
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
                    rw.Message = "Invalid email or password.";
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