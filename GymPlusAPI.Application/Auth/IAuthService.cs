using System;
using GymPlusAPI.Domain.Entities;

namespace GymPlusAPI.Application.Auth;

public interface IAuthService
{
    Task<User?> LoginAsync(string email, string password);
}
