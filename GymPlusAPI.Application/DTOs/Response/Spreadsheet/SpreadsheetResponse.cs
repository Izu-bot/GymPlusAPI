using System;
using GymPlusAPI.Application.DTOs.Response.Workout;

namespace GymPlusAPI.Application.DTOs.Response.Spreadsheet;

public record SpreadsheetResponse(
    int Id,
    string Name,
    List<WorkoutResponse> Workouts
);
