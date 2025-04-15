using System;
using GymPlusAPI.Application.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt; // Alias para evitar conflito de nome

namespace GymPlusAPI.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BCryptNet.HashPassword(password); // Gera um hash com salt automatico
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        try
        {
            // Verifica se a senha fornecida corresponde ao hash
            return BCryptNet.Verify(providedPassword, hashedPassword);
        }
        catch (BCrypt.Net.SaltParseException) // Trata caso de hash invalido
        {
            return false;
        }
    }
}
