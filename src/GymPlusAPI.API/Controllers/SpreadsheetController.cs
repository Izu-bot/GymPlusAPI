using System.Security.Claims;
using GymPlusAPI.API.Filters;
using GymPlusAPI.Application.DTOs.Request.Spreadsheet;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymPlusAPI.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class SpreadsheetController(ISpreadsheetService spreadsheetService) : ControllerBase
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateSpreadsheetRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var userId = GetClaimUserIdFormClaims();

            var spreadsheet = await spreadsheetService.CreateAsync(request, userId);

            return CreatedAtAction(nameof(GetByid), new { id = spreadsheet.Id }, spreadsheet);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByid(int id)
        {
            if (!ModelState.IsValid)
                return NotFound(ModelState);

            var userId = GetClaimUserIdFormClaims();
            
            var spreadsheet = await spreadsheetService.GetByIdAsync(id, userId);

            return Ok(spreadsheet);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var userId = GetClaimUserIdFormClaims();
            
            var spreadsheets = await spreadsheetService.GetAllAsync(userId);

            return Ok(spreadsheets);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSpreadsheetRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var userId = GetClaimUserIdFormClaims();
            
            await spreadsheetService.UpdateAsync(request, userId);

            return NoContent();
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
                
            await spreadsheetService.DeleteAsync(id, userId);

            return NoContent();
        }
    }
}