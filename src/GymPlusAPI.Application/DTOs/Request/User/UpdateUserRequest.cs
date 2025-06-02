using System;

namespace GymPlusAPI.Application.DTOs.Request.User;

public record UpdateUserRequest(
    Guid Id,
    string? Email,
    string? Password,
    string? Name
);
