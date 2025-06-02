using System;
using GymPlusAPI.Domain.Entities;

namespace GymPlusAPI.Domain.Interfaces;

public interface IWorkoutRepository
{
    Task<Workout?> GetWorkoutByIdAsync(int id, Guid userId);
    Task<IEnumerable<Workout>> GetWorkoutsByUserAsync(Guid userId);
    Task<IEnumerable<Workout>> GetWorkoutBySpreadsheetIdAsync(int spreadsheetId, Guid userId);
    Task AddAsync(Workout workout);
    Task UpdateAsync(Workout workout);
    Task DeleteAsync(Workout workout);
}
