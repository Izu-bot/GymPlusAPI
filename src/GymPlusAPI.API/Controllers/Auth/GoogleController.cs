using Google.Apis.Auth;
using GymPlusAPI.API.Filters;
using GymPlusAPI.Application.Auth;
using GymPlusAPI.Application.DTOs.Request.User;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = GymPlusAPI.Application.DTOs.Request.Login.LoginRequest;

namespace GymPlusAPI.API.Controllers.Auth;

// DTO (Data Transfer Object) para receber o token do app mobile
public class GoogleTokenRequest
{
    public string IdToken { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
[TypeFilter(typeof(CustomExceptionFilter))]
public class GoogleController(IConfiguration configuration, IJwtGenerator jwtGenerator, IUserService userService, IUserRepository userRepository) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IJwtGenerator _jwtGenerator = jwtGenerator;
    private readonly IUserService _userService = userService;
    private readonly IUserRepository _userRepository = userRepository;

    [HttpPost("callback")]
    public async Task<IActionResult> GoogleSignIn([FromBody] GoogleTokenRequest request)
    {
        if (string.IsNullOrEmpty(request.IdToken))
        {
            return BadRequest("IdToken is required");
        }

        try
        {
            var clientId = _configuration["Google:ClientId"];
            var validationSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                // O audience deve ser exatamente o Client ID da aplicação web no Google Cloud
                Audience = [clientId]
            };

            // Valida o token recebido usando a biblioteca do Google
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, validationSettings);

            // Se a validação for bem-sucedida, o 'payload' conterá as informações do usuário
            var existingUser = await _userRepository.GetUserByEmailAsync(payload.Email);

            if (existingUser == null)
            {
                var newUser = new User(
                    payload.Email,
                    "",
                    payload.Name,
                    "User"
                    );

                await _userRepository.AddAsync(newUser);
                existingUser = newUser;
            }
            
            // Gera o token JWT da aplicação para este usuário
            var appToken = _jwtGenerator.GenerateToken(existingUser);
            
            // Retorne o token para o app mobile
            return Ok(new { token = appToken, user = existingUser});
        }
        catch (InvalidJwtException ex)
        {
            // Ocorre se o token for inválido, expirado, ou a assinatura não corresponder
            return Unauthorized(new { message = "Token do Google inválido.", error = ex.Message });
        }
        catch (Exception ex)
        {
            // Erro genérico
            return StatusCode(500, new { message = "Ocorreu um erro interno no servidor.", error = ex.Message });
        }
    }
}