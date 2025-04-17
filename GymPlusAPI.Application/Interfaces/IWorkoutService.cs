using System;
using GymPlusAPI.Application.DTOs.Request.Workout;
using GymPlusAPI.Application.DTOs.Response.Workout;

namespace GymPlusAPI.Application.Interfaces;

public interface IWorkoutService
{
    Task<WorkoutResponse> CreateAsync(CreateWorkoutRequest dto, Guid userId);
    Task UpdateAsync(UpdateWorkoutRequest dto, Guid userId);
    Task DeleteAsync(int workoutId, Guid userId);
    Task<IEnumerable<WorkoutResponse>> GetAllAsync(Guid userId);
    Task<WorkoutResponse> GetByIdAsync(int workoutId, Guid userId);
    Task<IEnumerable<WorkoutResponse>> GetWorkoutBySpreadsheetAsync(int spreadsheetId, Guid userId);
}
