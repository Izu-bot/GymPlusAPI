using System;

namespace GymPlusAPI.Application.DTOs.Response.User;

public record UserResponse(
    Guid Id,
    string Email,
    string Name
);
