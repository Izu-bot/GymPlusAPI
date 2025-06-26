using System.Security.Claims;
using GymPlusAPI.API.Filters;
using GymPlusAPI.Application.DTOs.Request.Workout;
using GymPlusAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymPlusAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class WorkoutController(IWorkoutService workoutService) : ControllerBase
    {

        private Guid GetClaimUserIdFormClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;
            
            if (!Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException();

            return userId;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateWorkoutRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetClaimUserIdFormClaims();

            var workout = await workoutService.CreateAsync(request, userId);

            return CreatedAtAction(nameof(GetById), new { id = workout.Id }, workout);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
                return NotFound(ModelState);

            var userId = GetClaimUserIdFormClaims();

            var workout = await workoutService.GetByIdAsync(id, userId);

            return Ok(workout);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return NotFound(ModelState);

            var userId = GetClaimUserIdFormClaims();

            var workouts = await workoutService.GetAllAsync(userId);

            return Ok(workouts);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkoutRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetClaimUserIdFormClaims();

            var workout = await workoutService.GetByIdAsync(id, userId);

            
            await workoutService.UpdateAsync(request, userId);

            return Ok(workout);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var userId = GetClaimUserIdFormClaims();

            await workoutService.DeleteAsync(id, userId);

            return NoContent();
        }
    }
}