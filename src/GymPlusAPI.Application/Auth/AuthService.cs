using System;
using System.ComponentModel.DataAnnotations;
using GymPlusAPI.Application.DTOs.Request.Login;
using GymPlusAPI.Application.DTOs.Response.Login;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Domain.Interfaces;

namespace GymPlusAPI.Application.Auth;

public class AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtGenerator jwtGenerator)
    : IAuthService
{
    public async Task<LoginResponse?> LoginAsync(LoginRequest login)
    {
        var user = await userRepository.GetUserByEmailAsync(login.Email);

        if (user is null || !passwordHasher.VerifyHashedPassword(user.Password, login.Password))
           throw new ValidationException("Email ou senha invalidos");

        return new LoginResponse(jwtGenerator.GenerateToken(user));
    }
}
