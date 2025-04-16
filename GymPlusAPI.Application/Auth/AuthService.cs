using System;
using GymPlusAPI.Application.DTOs.Login;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;

namespace GymPlusAPI.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    // private readonly IJwtGenerator _jwtGenerator;
    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        // _jwtGenerator = jwtGenerator;
    }

    public async Task<User?> LoginAsync(LoginDTO login)
    {
        var user = await _userRepository.GetUserByEmailAsync(login.Email);

        if (user is null || !_passwordHasher.VerifyHashedPassword(user.Password, login.Password))
            return null;
        
        return user;
    }
}
