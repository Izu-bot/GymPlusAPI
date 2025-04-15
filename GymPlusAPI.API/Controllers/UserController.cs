using GymPlusAPI.Application.DTOs.User;
using GymPlusAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymPlusAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJwtGenerator _jwtGenerator;
        public UserController(IUserService userService, IJwtGenerator jwtGenerator)
        {
            _userService = userService;
            _jwtGenerator = jwtGenerator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.AddAsync(dto);

            var token = _jwtGenerator.GenerateToken(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, new { Token = token });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado.");
            
            return Ok(user);
        }
    }
}
