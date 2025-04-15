using GymPlusAPI.Application.Auth;
using GymPlusAPI.Application.DTOs.Login;
using GymPlusAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymPlusAPI.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IJwtGenerator _jwtGenerator;
        public AuthController(IAuthService authService, IJwtGenerator jwtGenerator)
        {
            _authService = authService;
            _jwtGenerator = jwtGenerator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _authService.LoginAsync(loginDTO.Email, loginDTO.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var token = _jwtGenerator.GenerateToken(user);

            return Ok(new { Token = token });
        }

    }
}
