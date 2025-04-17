using System;

namespace GymPlusAPI.Application.DTOs.Response.Login;

public record LoginResponse(
    string? AccessToken
);