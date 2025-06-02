using System;

namespace GymPlusAPI.Application.DTOs.Response.Workout;

public record WorkoutResponse(
    int Id,
    string Name,
    int Reps,
    int Series,
    int Weight
);
