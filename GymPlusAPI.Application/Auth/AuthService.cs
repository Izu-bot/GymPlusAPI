using System;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;

namespace GymPlusAPI.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    // private readonly IJwtGenerator _jwtGenerator;
    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<User?> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);

        if (user == null)
            return null;
        
        return user;
    }
}
