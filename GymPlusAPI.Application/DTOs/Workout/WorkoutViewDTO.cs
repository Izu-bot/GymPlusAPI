using System;

namespace GymPlusAPI.Application.DTOs.Workout;

public record WorkoutViewDTO(
    int Id,
    string Name,
    int Reps,
    int Series,
    int Weight
);
