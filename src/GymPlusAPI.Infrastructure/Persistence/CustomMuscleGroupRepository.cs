using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;
using GymPlusAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymPlusAPI.Infrastructure.Persistence;

public class CustomMuscleGroupRepository(AppDbContext context) : ICustomMuscleGroupRepository
{
    public async Task AddCustomGroup(CustomMuscleGroup customMuscleGroup)
    {
        await context.CustomMuscleGroups.AddAsync(customMuscleGroup);
        await context.SaveChangesAsync(); 
    }

    public async Task Update(CustomMuscleGroup customMuscleGroup)
    {
        context.CustomMuscleGroups.Update(customMuscleGroup);
        await context.SaveChangesAsync();
    }

    public async Task Remove(CustomMuscleGroup customMuscleGroup)
    {
        context.CustomMuscleGroups.Remove(customMuscleGroup);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistByName(string name, Guid userId)
    {
        return await context.CustomMuscleGroups
            .AnyAsync(g => g.Name == name && g.UserId == userId);
    }

    public async Task<int> GetNextBitValue(Guid userId)
    {
        var maxBitValue = await context.CustomMuscleGroups
            .Where(g => g.UserId == userId && g.BitValue >= 1024)
            .MaxAsync(g => (int?)g.BitValue) ?? (1 << 10) / 2;

        return maxBitValue << 1;
    }

    public async Task<CustomMuscleGroup?> GetById(int id, Guid userId)
    {
        return await context.CustomMuscleGroups.AsNoTracking().FirstOrDefaultAsync(cmg => cmg.Id == id && cmg.UserId == userId);
    }

    public async Task<IReadOnlyDictionary<string, CustomMuscleGroup>> GetAll(Guid userId)
    {
        return await context.CustomMuscleGroups
            .Where(g => g.UserId == userId)
            .AsNoTracking()
            .ToDictionaryAsync(pair => pair.Name, pair => pair);
    }

    public async Task<int> GetCombinedBitValue(IEnumerable<string> groupNames, Guid userId)
    {
        return await context.CustomMuscleGroups
            .Where(g => groupNames.Contains(g.Name) && g.UserId == userId)
            .SumAsync(g => g.BitValue);
    }
}