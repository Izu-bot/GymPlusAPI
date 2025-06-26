namespace GymPlusAPI.Application.DTOs.Response.RecurrentTraining;

public record RecurrentTrainingResponse(
    Guid Id,
    String Date,
    bool IsCompleted,
    string? Description
    );