using System;

namespace GymPlusAPI.Application.DTOs.Workout;

public record WorkoutCreateDTO(
    string Name,
    int Reps,
    int Series,
    int Weight,
    int SpreadsheetId
);