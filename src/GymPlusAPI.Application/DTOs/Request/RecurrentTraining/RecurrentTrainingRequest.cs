namespace GymPlusAPI.Application.DTOs.Request.RecurrentTraining;

public record RecurrentTrainingRequest(
    bool IsCompleted,
    string? Description
    );