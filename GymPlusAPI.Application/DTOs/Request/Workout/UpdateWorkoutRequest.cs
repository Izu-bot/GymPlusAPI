using System;

namespace GymPlusAPI.Application.DTOs.Request.Workout;

public record UpdateWorkoutRequest(
    int Id,
    string Name,
    int Reps,
    int Series,
    int Weight,
    int SpreadsheetId
);
