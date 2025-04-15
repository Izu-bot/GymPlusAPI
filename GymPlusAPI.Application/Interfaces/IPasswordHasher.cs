using System;

namespace GymPlusAPI.Application.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}
