using System;

namespace GymPlusAPI.Application.DTOs.User;

public record UserCreateDTO(
    string Username,
    string Password,
    string Name
);