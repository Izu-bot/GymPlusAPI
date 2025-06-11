namespace GymPlusAPI.Application.DTOs.Response.CustomMuscleGroup;

public record CustomMuscleGroupResponse(
    int Id,
    string Name,
    int  BitValue,
    Guid? UserId,
    int? SpreadsheetId
    );