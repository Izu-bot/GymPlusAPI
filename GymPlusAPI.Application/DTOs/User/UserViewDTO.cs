using System;

namespace GymPlusAPI.Application.DTOs.User;

public record UserViewDTO(
    string Username,
    string Password,
    string Name
);