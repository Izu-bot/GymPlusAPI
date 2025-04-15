using System;

namespace GymPlusAPI.Application.DTOs.Login;

public record LoginDTO(
    string Email,
    string Password
);