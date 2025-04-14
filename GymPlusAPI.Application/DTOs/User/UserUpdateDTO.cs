using System;

namespace GymPlusAPI.Application.DTOs.User;

public record UserUpdateDTO(
    Guid Id,
    string Username,
    string Password,
    string Name
);