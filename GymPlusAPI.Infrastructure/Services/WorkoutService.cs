using System;
using GymPlusAPI.Application.DTOs.Workout;
using GymPlusAPI.Application.Services;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;

namespace GymPlusAPI.Infrastructure.Services;

public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly ISpreadsheetRepository _spreadsheetRepository;
    public WorkoutService(IWorkoutRepository workoutRepository, ISpreadsheetRepository spreadsheetRepository)
    {
        _workoutRepository = workoutRepository;
        _spreadsheetRepository = spreadsheetRepository;
    }

    public async Task<int> CreateAsync(WorkoutCreateDTO dto, Guid userId)
    {
        var spreadsheet = await _spreadsheetRepository.GetSpreadsheetByIdAsync(dto.SpreadsheetId, userId);

        if (spreadsheet == null)
            throw new Exception("Planilha não encontrada.");

        var workout = new Workout
        {
            Name = dto.Name,
            Reps = dto.Reps,
            Series = dto.Series,
            Weight = dto.Weight,
            SpreadsheetId = dto.SpreadsheetId
        };

        await _workoutRepository.AddAsync(workout);

        return workout.Id;
    }


    public async Task UpdateAsync(WorkoutUpdateDTO dto, Guid userId)
    {
        var workoutToUpdate = await _workoutRepository.GetWorkoutByIdAsync(dto.Id, userId) ?? throw new Exception("Exercício não encontrado.");

        workoutToUpdate.Name = dto.Name;
        workoutToUpdate.Reps = dto.Reps;
        workoutToUpdate.Series = dto.Series;
        workoutToUpdate.Weight = dto.Weight;
        workoutToUpdate.SpreadsheetId = dto.SpreadsheetId;

        await _workoutRepository.UpdateAsync(workoutToUpdate);
    }

    public async Task DeleteAsync(int workoutId, Guid userId)
    {
        var workoutToDelete = await _workoutRepository.GetWorkoutByIdAsync(workoutId, userId) ?? throw new Exception("Exercício não encontrado.");

        await _workoutRepository.DeleteAsync(workoutToDelete);
    }

    public async Task<IEnumerable<WorkoutViewDTO>> GetAllAsync(Guid userId)
    {
        var workouts = await _workoutRepository.GetWorkoutsByUserAsync(userId);

        return workouts.Select(w => new WorkoutViewDTO(
            w.Id,
            w.Name,
            w.Reps,
            w.Series,
            w.Weight
        ));
    }

    public async Task<WorkoutViewDTO> GetByIdAsync(int workoutId, Guid userId)
    {
        var workout = await _workoutRepository.GetWorkoutByIdAsync(workoutId, userId) ?? throw new Exception("Exercício não encontrado.");

        return new WorkoutViewDTO(
            workout.Id,
            workout.Name,
            workout.Reps,
            workout.Series,
            workout.Weight
        );
    }

    public async Task<IEnumerable<WorkoutViewDTO>> GetWorkoutBySpreadsheetAsync(int spreadsheetId, Guid userId)
    {
        var workouts = await _workoutRepository.GetWorkoutBySpreadsheetIdAsync(spreadsheetId, userId);

        return workouts.Select(w => new WorkoutViewDTO(
            w.Id,
            w.Name,
            w.Reps,
            w.Series,
            w.Weight
        ));
    }
}
