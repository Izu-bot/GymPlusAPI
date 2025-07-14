using System;

namespace GymPlusAPI.Application.DTOs.Request.Login;

public record LoginRequest(
    string Email,
    string? Password
);
