using System;
using GymPlusAPI.Application.DTOs.Request.Workout;
using GymPlusAPI.Application.DTOs.Response.Workout;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;

namespace GymPlusAPI.Application.Services;

public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly ISpreadsheetRepository _spreadsheetRepository;
    public WorkoutService(IWorkoutRepository workoutRepository, ISpreadsheetRepository spreadsheetRepository)
    {
        _workoutRepository = workoutRepository;
        _spreadsheetRepository = spreadsheetRepository;
    }

    public async Task<WorkoutResponse> CreateAsync(CreateWorkoutRequest dto, Guid userId)
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

        return new WorkoutResponse(
            workout.Id,
            workout.Name,
            workout.Reps,
            workout.Series,
            workout.Weight
        );
    }


    public async Task UpdateAsync(UpdateWorkoutRequest dto, Guid userId)
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

    public async Task<IEnumerable<WorkoutResponse>> GetAllAsync(Guid userId)
    {
        var workouts = await _workoutRepository.GetWorkoutsByUserAsync(userId);

        return workouts.Select(w => new WorkoutResponse(
            w.Id,
            w.Name,
            w.Reps,
            w.Series,
            w.Weight
        ));
    }

    public async Task<WorkoutResponse> GetByIdAsync(int workoutId, Guid userId)
    {
        var workout = await _workoutRepository.GetWorkoutByIdAsync(workoutId, userId) ?? throw new Exception("Exercício não encontrado.");

        return new WorkoutResponse(
            workout.Id,
            workout.Name,
            workout.Reps,
            workout.Series,
            workout.Weight
        );
    }

    public async Task<IEnumerable<WorkoutResponse>> GetWorkoutBySpreadsheetAsync(int spreadsheetId, Guid userId)
    {
        var workouts = await _workoutRepository.GetWorkoutBySpreadsheetIdAsync(spreadsheetId, userId);

        return workouts.Select(w => new WorkoutResponse(
            w.Id,
            w.Name,
            w.Reps,
            w.Series,
            w.Weight
        ));
    }
}
