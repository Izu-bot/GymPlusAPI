using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;
using GymPlusAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymPlusAPI.Infrastructure.Persistence;

public class SpreadsheetRepository(AppDbContext context) : ISpreadsheetRepository
{
    public async Task AddAsync(Spreadsheet spreadsheet)
    {
        if (spreadsheet == null)
        {
            throw new ArgumentNullException(nameof(spreadsheet));
        }

        await context.Spreadsheets.AddAsync(spreadsheet);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Spreadsheet spreadsheet)
    {
        if (spreadsheet == null)
        {
            throw new ArgumentNullException(nameof(spreadsheet));
        }

        context.Spreadsheets.Remove(spreadsheet);
        await context.SaveChangesAsync();
    }

    public async Task<Spreadsheet?> GetSpreadsheetByIdAsync(int id, Guid userId)
    {
        return await context.Spreadsheets
            .Include(s => s.Workouts)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
    }

    public async Task<IEnumerable<Spreadsheet>> GetSpreadsheetsByUserAsync(Guid userId)
    {
        return await context.Spreadsheets
            .Where(s => s.UserId == userId)
            .Include(s => s.Workouts)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Spreadsheet>> TodaySpreadsheet(DayOfWeek dayOfWeek, Guid userId)
    {
        return await context.Spreadsheets
            .Where(s => 
                s.UserId == userId &&
                s.DaysOfWeek.Contains(dayOfWeek) &&
                !s.RecurrentTrainings.Any(rt => rt.UserId == userId && rt.IsCompleted))
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task UpdateAsync(Spreadsheet spreadsheet)
    {
        if (spreadsheet == null)
        {
            throw new ArgumentNullException(nameof(spreadsheet));
        }

        context.Spreadsheets.Update(spreadsheet);
        await context.SaveChangesAsync();
    }
}
