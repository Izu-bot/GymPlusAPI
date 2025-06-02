using System;

namespace GymPlusAPI.Application.DTOs.Request.User;

public record CreateUserRequest(
    string Email,
    string Password,
    string Name,
    string Role = "User" // Default role
);
