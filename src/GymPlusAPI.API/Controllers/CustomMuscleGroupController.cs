using System.Security.Claims;
using GymPlusAPI.API.Filters;
using GymPlusAPI.Application.DTOs.Request.CustomMuscleGroup;
using GymPlusAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymPlusAPI.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[TypeFilter(typeof(CustomExceptionFilter))]
public class CustomMuscleGroupController(ICustomMuscleGroupService customMuscleGroupService) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CustomMuscleGroupRequest request)
    {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var customMuscleGroup = await customMuscleGroupService.AddCustomGroup(request, userId);

            return CreatedAtAction(nameof(GetById), new { id = customMuscleGroup.Id }, customMuscleGroup);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
            if (!ModelState.IsValid)
                return NotFound(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var customMuscleGroup = await customMuscleGroupService.GetById(id, userId);

            return Ok(customMuscleGroup);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
            if (!ModelState.IsValid)
                return BadRequest();

            var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userClaimId, out var userId)) return Unauthorized();

            var customMuscleGroups = await customMuscleGroupService.GetAll(userId);

            return Ok(customMuscleGroups);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomMuscleGroupRequest request)
    {
            if (!ModelState.IsValid)
                return BadRequest();
        
            var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;
        
            if (!Guid.TryParse(userClaimId, out var userId)) return Unauthorized();

            await customMuscleGroupService.UpdateCustomGroup(request, userId);

            return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
            if (!ModelState.IsValid)
                    return BadRequest();

            var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;
            
            if (!Guid.TryParse(userClaimId, out var userId)) return Unauthorized();

            var muscleGroupToRemove = await customMuscleGroupService.GetById(id, userId);
            
            await customMuscleGroupService.RemoveCustomGroup(muscleGroupToRemove.Id, userId);

            return NoContent();
    }
}