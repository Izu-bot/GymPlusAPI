using System;
using GymPlusAPI.Application.DTOs.Login;
using GymPlusAPI.Domain.Entities;

namespace GymPlusAPI.Application.Auth;

public interface IAuthService
{
    Task<User?> LoginAsync(LoginDTO login);
}
