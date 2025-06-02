using System.Security.Claims;
using GymPlusAPI.Application.DTOs.Request.User;
using GymPlusAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymPlusAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userService.AddAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = user.Id },
                    user
                    );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById()
        {
            try
            {
                if(!ModelState.IsValid)
                    return NotFound(ModelState);

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? User.FindFirst("sub")?.Value;

                if(!Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized();
                
                var user = await _userService.GetByIdAsync(userId);

                if (user == null)
                    return NotFound(new { message = "Usuário não encontrado." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(UpdateUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                  ?? User.FindFirst("sub")?.Value;
                
                if (!Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized();
                
                var user = await _userService.GetByIdAsync(userId);

                if (user == null)
                    return NotFound(new { message = "Usuário não encontrado." });

                await _userService.UpdateAsync(request);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                  ?? User.FindFirst("sub")?.Value;
                
                if (!Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized();
                
                var user = await _userService.GetByIdAsync(userId);

                if (user == null)
                    return NotFound(new { message = "Usuário não encontrado." });

                await _userService.DeleteAsync(user.Id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
