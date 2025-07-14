using GymPlusAPI.Application.Auth;
using GymPlusAPI.Application.DTOs.Request.Login;
using GymPlusAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymPlusAPI.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtGenerator _jwtGenerator;
        public AuthController(IAuthService authService, IJwtGenerator jwtGenerator)
        {
            _authService = authService;
            _jwtGenerator = jwtGenerator;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _authService.LoginAsync(request);

                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }
                return Ok(new { token = user.AccessToken });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
