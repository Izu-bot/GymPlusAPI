using System;
using GymPlusAPI.Domain.Entities;

namespace GymPlusAPI.Application.Interfaces;

public interface IJwtGenerator
{
    string GenerateToken(User user);
}
