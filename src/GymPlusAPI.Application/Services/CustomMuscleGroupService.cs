
using GymPlusAPI.Application.DTOs.Request.CustomMuscleGroup;
using GymPlusAPI.Application.DTOs.Response.CustomMuscleGroup;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Exceptions;
using GymPlusAPI.Domain.Interfaces;

namespace GymPlusAPI.Application.Services;

public class CustomMuscleGroupService(ICustomMuscleGroupRepository repository) : ICustomMuscleGroupService
{
    public async Task<CustomMuscleGroupResponse> AddCustomGroup(CustomMuscleGroupRequest dto, Guid userId)
    {
        var exist = await repository.ExistByName(dto.Name, userId);

        if (exist)
            throw new InvalidOperationException($"Grupo {dto.Name} já existe");
        
        var nextBitValue = await repository.GetNextBitValue(userId);

        var newGroup = new CustomMuscleGroup(
            dto.Name,
            nextBitValue,
            userId);

        await repository.AddCustomGroup(newGroup);
        
        return new CustomMuscleGroupResponse(
            newGroup.Id,
            newGroup.Name,
            newGroup.BitValue
            );
    }

    public async Task<CustomMuscleGroupResponse> UpdateCustomGroup(UpdateCustomMuscleGroupRequest dto, Guid userId)
    {
        var muscleGroupToUpdate = await repository.GetById(dto.Id, userId) ?? throw new EntityNotFoundException("Grupo muscular não encontrado");
        
        muscleGroupToUpdate.Name = dto.Name;

        await repository.Update(muscleGroupToUpdate);

        return new CustomMuscleGroupResponse(
            muscleGroupToUpdate.Id,
            muscleGroupToUpdate.Name,
            muscleGroupToUpdate.BitValue
        );
    }

    public async Task RemoveCustomGroup(int customMucleGroup, Guid userId)
    {
        var mucleGroupToRemove = await repository.GetById(customMucleGroup, userId) ?? throw new EntityNotFoundException("Grupo muscular");
        
        await repository.Remove(mucleGroupToRemove);
    }

    public async Task<IReadOnlyDictionary<string, CustomMuscleGroup>> GetAll(Guid userId)
    {
        return await repository.GetAll(userId);
    }

    public async Task<int> GetCombinedBitValue(IEnumerable<string> groupNames, Guid userId)
    {
        return await repository.GetCombinedBitValue(groupNames, userId);
    }

    public async Task<CustomMuscleGroupResponse> GetById(int id, Guid userId)
    {
        var muscleGroup =  await repository.GetById(id,  userId);
        
        if (muscleGroup == null) throw new EntityNotFoundException("Grupo Muscular");

        return new CustomMuscleGroupResponse(
            muscleGroup.Id,
            muscleGroup.Name,
            muscleGroup.BitValue
        );
    }
}