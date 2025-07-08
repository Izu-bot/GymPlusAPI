using System.Security.Claims;
using GymPlusAPI.API.Filters;
using GymPlusAPI.Application.DTOs.Request.RecurrentTraining;
using GymPlusAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymPlusAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[TypeFilter(typeof(CustomExceptionFilter))]
public class RecurrentTrainingController(IRecurrentTrainingService recurrentTrainingService) : ControllerBase
{
    private Guid GetClaimUserIdFormClaims()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst("sub")?.Value;
        
        if (!Guid.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException();
        
        return userId;
    }

    // Nome do placeholder do id deve ser igual ao do parametro :D
    [HttpPost("{spreadsheetId}/recurrent-training")]
    public async Task<IActionResult> Create(int spreadsheetId, [FromBody] RecurrentTrainingRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var userId = GetClaimUserIdFormClaims();
        
        var recurrentTraining = await recurrentTrainingService.CreateAsync(spreadsheetId, request, userId);
        
        return Ok(recurrentTraining);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRecurrentTraining()
    {
        
        var userId = GetClaimUserIdFormClaims();
        
        var recurrentTraining = await recurrentTrainingService.GetRecurrentTraining(userId);
        
        return Ok(recurrentTraining);
    }
}