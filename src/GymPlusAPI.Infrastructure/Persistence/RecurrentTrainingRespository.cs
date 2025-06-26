using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;
using GymPlusAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymPlusAPI.Infrastructure.Persistence;

public class RecurrentTrainingRepository(AppDbContext context) : IRecurrentTrainingRepository
{
    
    public async Task AddAsync(RecurrentTraining recurrentTraining)
    {
        if (recurrentTraining == null)
            throw new ArgumentException(nameof(recurrentTraining));
        
        await context.RecurrentTrainings.AddAsync(recurrentTraining);
        await context.SaveChangesAsync();
    }

    public async Task<IReadOnlyDictionary<Spreadsheet, RecurrentTraining>> GetRecurrentTrainings(Guid userId)
    {
        return await context.RecurrentTrainings
            .Include(rt => rt.Spreadsheet)
            .Include(rt => rt.Spreadsheet.Workouts)
            .Where(rt => rt.UserId == userId)
            .AsNoTracking()
            .ToDictionaryAsync(rt => rt.Spreadsheet, rt => rt);
    }
}