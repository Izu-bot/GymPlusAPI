using System;
using GymPlusAPI.Application.DTOs.Request.Login;
using GymPlusAPI.Application.DTOs.Response.Login;
using GymPlusAPI.Domain.Entities;

namespace GymPlusAPI.Application.Auth;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest login);
}
