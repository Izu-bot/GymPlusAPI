using System;

namespace GymPlusAPI.Application.DTOs.Request.User;

public record GetUserByIdRequest(
    Guid Id
);
