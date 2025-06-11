using GymPlusAPI.Domain.Entities;

namespace GymPlusAPI.Domain.Interfaces;

public interface ICustomMuscleGroupRepository
{
    Task AddCustomGroup(CustomMuscleGroup customMuscleGroup);
    Task Update(CustomMuscleGroup customMuscleGroup);
    Task Remove(CustomMuscleGroup customMuscleGroup);
    Task<bool> ExistByName(string name, Guid userId);
    Task<int> GetNextBitValue(Guid userId);
    Task<CustomMuscleGroup?> GetById(int id, Guid userId);
    Task<IReadOnlyDictionary<string, CustomMuscleGroup>> GetAll(Guid userId);
    Task<int> GetCombinedBitValue(IEnumerable<string> groupNames,  Guid userId);
}