using System.Security.Claims;
using GymPlusAPI.Application.DTOs.Request.Spreadsheet;
using GymPlusAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymPlusAPI.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SpreadsheetController : Controller
    {
        private readonly ISpreadsheetService _spreadsheetService;

        public SpreadsheetController(ISpreadsheetService spreadsheetService)
        {
            _spreadsheetService = spreadsheetService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateSpreadsheetRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? User.FindFirst("sub")?.Value;

                if (!Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized();

                var spreadsheet = await _spreadsheetService.CreateAsync(request, userId);

                return CreatedAtAction(nameof(GetByid), new { id = spreadsheet.Id }, spreadsheet);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByid(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return NotFound(ModelState);

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? User.FindFirst("sub")?.Value;

                if (!Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized();

                var spreadsheet = await _spreadsheetService.GetByIdAsync(id, userId);

                if (spreadsheet == null)
                    return NotFound("Planilha não encontrada.");

                return Ok(spreadsheet);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? User.FindFirst("sub")?.Value;
                if (!Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized();

                var spreadsheets = await _spreadsheetService.GetAllAsync(userId);

                if (spreadsheets == null || !spreadsheets.Any())
                    return NotFound("Nenhuma planilha encontrada.");

                return Ok(spreadsheets);
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSpreadsheetRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? User.FindFirst("sub")?.Value;
                if (!Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized();

                var spreadsheet = await _spreadsheetService.GetByIdAsync(id, userId);

                if (spreadsheet == null)
                    return NotFound("Planilha não encontrada.");

                await _spreadsheetService.UpdateAsync(request, userId);

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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? User.FindFirst("sub")?.Value;
                if (!Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized();

                var spreadsheet = await _spreadsheetService.GetByIdAsync(id, userId);

                if (spreadsheet == null)
                    return NotFound("Planilha não encontrada.");

                await _spreadsheetService.DeleteAsync(id, userId);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
