using System;

namespace GymPlusAPI.Application.DTOs.User;

public record UserCreateDTO(
    string Email,
    string Password,
    string Name,
    string Role = "User" // Default role
);