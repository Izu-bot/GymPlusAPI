using System;

namespace GymPlusAPI.Application.DTOs.Request.Workout;

public record CreateWorkoutRequest(
    string Name,
    int Reps,
    int Series,
    int Weight,
    int SpreadsheetId
);
