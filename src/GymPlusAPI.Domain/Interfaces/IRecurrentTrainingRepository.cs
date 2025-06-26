using GymPlusAPI.Domain.Entities;

namespace GymPlusAPI.Domain.Interfaces;

public interface IRecurrentTrainingRepository
{
    Task AddAsync(RecurrentTraining recurrentTraining);
    Task<IReadOnlyDictionary<Spreadsheet, RecurrentTraining>> GetRecurrentTrainings(Guid userId);
}