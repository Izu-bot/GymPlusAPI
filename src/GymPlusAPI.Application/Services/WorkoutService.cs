using System;
using GymPlusAPI.Application.DTOs.Request.Workout;
using GymPlusAPI.Application.DTOs.Response.Workout;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Exceptions;
using GymPlusAPI.Domain.Interfaces;

namespace GymPlusAPI.Application.Services;

public class WorkoutService(IWorkoutRepository workoutRepository, ISpreadsheetRepository spreadsheetRepository)
    : IWorkoutService
{
    public async Task<WorkoutResponse> CreateAsync(CreateWorkoutRequest dto, Guid userId)
    {
        var spreadsheet = await spreadsheetRepository.GetSpreadsheetByIdAsync(dto.SpreadsheetId, userId);

        if (spreadsheet == null)
            throw new EntityNotFoundException("Planilha");

        var workout = new Workout
        {
            Name = dto.Name,
            Reps = dto.Reps,
            Series = dto.Series,
            Weight = dto.Weight,
            SpreadsheetId = dto.SpreadsheetId
        };

        await workoutRepository.AddAsync(workout);

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
        var workoutToUpdate = await workoutRepository.GetWorkoutByIdAsync(dto.Id, userId) ?? throw new EntityNotFoundException("Exercício");

        workoutToUpdate.Name = dto.Name;
        workoutToUpdate.Reps = dto.Reps;
        workoutToUpdate.Series = dto.Series;
        workoutToUpdate.Weight = dto.Weight;
        workoutToUpdate.SpreadsheetId = dto.SpreadsheetId;

        await workoutRepository.UpdateAsync(workoutToUpdate);
    }

    public async Task DeleteAsync(int workoutId, Guid userId)
    {
        var workoutToDelete = await workoutRepository.GetWorkoutByIdAsync(workoutId, userId) ?? throw new EntityNotFoundException("Exercício");

        await workoutRepository.DeleteAsync(workoutToDelete);
    }

    public async Task<IEnumerable<WorkoutResponse>> GetAllAsync(Guid userId)
    {
        var workouts = (await workoutRepository.GetWorkoutsByUserAsync(userId)).ToList();

        if (!workouts.Any())
            throw new EntityNotFoundException("Exercício");
        
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
        var workout = await workoutRepository.GetWorkoutByIdAsync(workoutId, userId) ?? throw new EntityNotFoundException("Exercício");

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
        var workouts = await workoutRepository.GetWorkoutBySpreadsheetIdAsync(spreadsheetId, userId);

        return workouts.Select(w => new WorkoutResponse(
            w.Id,
            w.Name,
            w.Reps,
            w.Series,
            w.Weight
        ));
    }
}
