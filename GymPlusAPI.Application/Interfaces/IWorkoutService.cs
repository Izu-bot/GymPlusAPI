using System;
using GymPlusAPI.Application.DTOs.Workout;

namespace GymPlusAPI.Application.Interfaces;

public interface IWorkoutService
{
    Task<int> CreateAsync(WorkoutCreateDTO dto, Guid userId);
    Task UpdateAsync(WorkoutUpdateDTO dto, Guid userId);
    Task DeleteAsync(int workoutId, Guid userId);
    Task<IEnumerable<WorkoutViewDTO>> GetAllAsync(Guid userId);
    Task<WorkoutViewDTO> GetByIdAsync(int workoutId, Guid userId);
    Task<IEnumerable<WorkoutViewDTO>> GetWorkoutBySpreadsheetAsync(int spreadsheetId, Guid userId);
}
