using System;
using GymPlusAPI.Application.DTOs.Request.Login;
using GymPlusAPI.Application.DTOs.Response.Login;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Domain.Interfaces;

namespace GymPlusAPI.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtGenerator _jwtGenerator;
    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtGenerator jwtGenerator)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest login)
    {
        var user = await _userRepository.GetUserByEmailAsync(login.Email);

        if (user is null || !_passwordHasher.VerifyHashedPassword(user.Password, login.Password))
            return null;

        return new LoginResponse(_jwtGenerator.GenerateToken(user));
    }
}
