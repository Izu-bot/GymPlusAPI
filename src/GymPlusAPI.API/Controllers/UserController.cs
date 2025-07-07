using System.Security.Claims;
using GymPlusAPI.API.Filters;
using GymPlusAPI.Application.DTOs.Request.User;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GymPlusAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class UserController(IUserService userService) : ControllerBase
    {

        private Guid GetClaimUserIdFormClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId)) throw new UnauthorizedAccessException();
            return userId;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userService.AddAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = user.Id },
                user
            );
        }

        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById()
        {
            if(!ModelState.IsValid)
                return NotFound(ModelState);
                
            var userId = GetClaimUserIdFormClaims();
            await userService.GetByIdAsync(userId);
                
            return Ok(userId);
        }

        [HttpPut("me")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var userId = GetClaimUserIdFormClaims();
            var updatedRequest = request with { Id = userId };
            await userService.UpdateAsync(updatedRequest);

            return NoContent();
        }

        [HttpDelete("me")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete()
        {
           
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;
            
            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();
            
            await userService.DeleteAsync(userId);
            
            return NoContent();
        }
    }
}