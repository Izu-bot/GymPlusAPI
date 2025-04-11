using System;
using GymPlusAPI.Application.DTOs.Workout;

namespace GymPlusAPI.Application.DTOs.Spreadsheet;

public record SpreadsheetViewDTO(
    int Id,
    string Name,
    List<WorkoutViewDTO> Workouts
);