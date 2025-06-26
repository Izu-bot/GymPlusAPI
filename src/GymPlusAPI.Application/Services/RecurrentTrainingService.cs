using GymPlusAPI.Application.DTOs.Request.RecurrentTraining;
using GymPlusAPI.Application.DTOs.Response.CustomMuscleGroup;
using GymPlusAPI.Application.DTOs.Response.RecurrentTraining;
using GymPlusAPI.Application.DTOs.Response.Spreadsheet;
using GymPlusAPI.Application.DTOs.Response.Workout;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;

namespace GymPlusAPI.Application.Services;

public class RecurrentTrainingService(IRecurrentTrainingRepository recurrentTrainingRepository) : IRecurrentTrainingService
{
    public async Task<RecurrentTrainingResponse> CreateAsync(int spreadsheetId, RecurrentTrainingRequest dto, Guid userId)
    {
        var createRecurrentTraining = new RecurrentTraining(dto.IsCompleted, userId, spreadsheetId, dto.Description);
        
        await recurrentTrainingRepository.AddAsync(createRecurrentTraining);

        return new RecurrentTrainingResponse(
            createRecurrentTraining.Id,
            createRecurrentTraining.Date.ToString("d"),
            createRecurrentTraining.IsCompleted,
            createRecurrentTraining.Observations
        );
    }

    public async Task<List<KeyValuePair<SpreadsheetResponse, RecurrentTrainingResponse>>> GetRecurrentTraining(Guid userId)
    {
        var entities = await recurrentTrainingRepository.GetRecurrentTrainings(userId);

        var result = entities.ToDictionary(
            pair => new SpreadsheetResponse(
                pair.Key.Id,
                pair.Key.Name,
                pair.Key.Description,
                pair.Key.IsRecurring,
                pair.Key.DaysOfWeek,
                pair.Key.Workouts.Select(w => new WorkoutResponse(
                    w.Id,
                    w.Name,
                    w.Reps,
                    w.Series,
                    w.Weight
                    )).ToList(),
                pair.Key.CustomMuscleGroups.Select(cms => new CustomMuscleGroupResponse(
                    cms.Id,
                    cms.Name,
                    cms.BitValue
                    )).ToList()
                ),
            pair => new RecurrentTrainingResponse(
                pair.Value.Id,
                pair.Value.Date.ToString("dd/MM/YYYY"),
                pair.Value.IsCompleted,
                pair.Value.Observations
                )
        ).ToList();

        return result;
    }
}