using System;
using GymPlusAPI.Application.DTOs.Response.CustomMuscleGroup;
using GymPlusAPI.Application.DTOs.Response.Workout;
using GymPlusAPI.Domain.Entities;

namespace GymPlusAPI.Application.DTOs.Response.Spreadsheet;

public record SpreadsheetResponse(
    int Id,
    string Name,
    string? Description,
    bool IsRecurring,
    List<DayOfWeek> DaysOfWeek,
    List<WorkoutResponse> Workouts,
    List<CustomMuscleGroupResponse> CustomMuscleGroups
);
