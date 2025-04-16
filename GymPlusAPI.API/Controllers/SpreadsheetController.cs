using GymPlusAPI.Application.DTOs.Spreadsheet;
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
        public async Task<IActionResult> Create([FromBody] SpreadsheetCreateDTO dto, [FromHeader] Guid userId)
        {
           if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _spreadsheetService.CreateAsync(dto, userId);

            return CreatedAtAction(nameof(GetByid), new { id }, null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByid(int id, [FromHeader] Guid userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var spreadsheet = await _spreadsheetService.GetByIdAsync(id, userId);

            if (spreadsheet == null)
                return NotFound("Planilha não encontrada.");

            return Ok(spreadsheet);
        }
    }
}
