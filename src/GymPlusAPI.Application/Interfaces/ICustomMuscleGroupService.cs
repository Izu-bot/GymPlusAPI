using GymPlusAPI.Application.DTOs.Request.CustomMuscleGroup;
using GymPlusAPI.Application.DTOs.Response.CustomMuscleGroup;
using GymPlusAPI.Domain.Entities;

namespace GymPlusAPI.Application.Interfaces;

public interface ICustomMuscleGroupService
{
    Task<CustomMuscleGroupResponse> AddCustomGroup(CustomMuscleGroupRequest dto, Guid userId);
    Task<CustomMuscleGroupResponse> UpdateCustomGroup(UpdateCustomMuscleGroupRequest dto, Guid userId);
    Task RemoveCustomGroup(int customMuscleGroup, Guid userId);
    Task<IReadOnlyDictionary<string, CustomMuscleGroup>> GetAll(Guid userId);
    Task<int> GetCombinedBitValue(IEnumerable<string> groupNames,  Guid userId);
    Task<CustomMuscleGroupResponse> GetById(int id, Guid userId);
}