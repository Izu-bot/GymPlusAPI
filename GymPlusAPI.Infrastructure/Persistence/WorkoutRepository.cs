using System;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;
using GymPlusAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymPlusAPI.Infrastructure.Persistence;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly AppDbContext _context;
    public WorkoutRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(Workout workout)
    {
        if (workout == null) throw new ArgumentNullException(nameof(workout));

        await _context.Workouts.AddAsync(workout);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Workout workout, Guid userId)
    {
        if (workout == null) throw new ArgumentNullException(nameof(workout));

        // Garante que o treino pertence ao usuário via Spreadsheet
        var existing = await _context.Workouts
            .Include(w => w.Spreadsheet)
            .FirstOrDefaultAsync(w => w.Id == workout.Id && w.Spreadsheet.UserId == userId);

        if (existing == null)
            throw new InvalidOperationException("Workout not found or does not belong to the user.");

        _context.Workouts.Remove(existing);
        await _context.SaveChangesAsync();
    }

    public async Task<Workout?> GetWorkoutByIdAsync(int id, Guid userId)
    {
        return await _context.Workouts
            .Include(w => w.Spreadsheet)
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id && w.Spreadsheet.UserId == userId);
    }

    public async Task<IEnumerable<Workout>> GetWorkoutBySpreadsheetIdAsync(int spreadsheetId, Guid userId)
    {
        return await _context.Workouts
            .Where(w => w.SpreadsheetId == spreadsheetId && w.Spreadsheet.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Workout>> GetWorkoutsByUserAsync(Guid userId)
    {
        return await _context.Workouts
            .Include(w => w.Spreadsheet)
            .Where(w => w.Spreadsheet.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(Workout workout)
    {
        if (workout == null) throw new ArgumentNullException(nameof(workout));

        _context.Workouts.Update(workout);
        await _context.SaveChangesAsync();
    }
}
