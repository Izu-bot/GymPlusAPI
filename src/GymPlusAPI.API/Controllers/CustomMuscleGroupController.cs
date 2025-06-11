using System.Security.Claims;
using GymPlusAPI.Application.DTOs.Request.CustomMuscleGroup;
using GymPlusAPI.Application.DTOs.Response.CustomMuscleGroup;
using GymPlusAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GymPlusAPI.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CustomMuscleGroupController(ICustomMuscleGroupService customMuscleGroupService) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CustomMuscleGroupRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var customMuscleGroup = await customMuscleGroupService.AddCustomGroup(request, userId);

            return CreatedAtAction(nameof(GetById), new { id = customMuscleGroup.Id }, customMuscleGroup);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message});
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            if (!ModelState.IsValid)
                return NotFound(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var customMuscleGroup = await customMuscleGroupService.GetById(id, userId);

            return Ok(customMuscleGroup);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userClaimId, out var userId)) return Unauthorized();

            var customMuscleGroups = await customMuscleGroupService.GetAll(userId);

            return Ok(customMuscleGroups);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomMuscleGroupRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest();
        
            var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;
        
            if (!Guid.TryParse(userClaimId, out var userId)) return Unauthorized();

            var _ = await customMuscleGroupService.GetById(id, userId) ?? throw new Exception("Grupo muscular não existe");

            await customMuscleGroupService.UpdateCustomGroup(request, userId);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest( new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (!ModelState.IsValid)
                    return BadRequest();

            var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;
            
            if (!Guid.TryParse(userClaimId, out var userId)) return Unauthorized();

            var muscleGroupToRemove = await customMuscleGroupService.GetById(id, userId) ?? throw new Exception("Grupo não existe");
            
            await customMuscleGroupService.RemoveCustomGroup(muscleGroupToRemove.Id, userId);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message =  ex.Message });
        }   
    }
}