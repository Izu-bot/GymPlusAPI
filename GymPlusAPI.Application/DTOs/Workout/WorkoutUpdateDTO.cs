using System;

namespace GymPlusAPI.Application.DTOs.Workout;

public record WorkoutUpdateDTO(
    int Id,
    string Name,
    int Reps,
    int Series,
    int Weight,
    int SpreadsheetId
);