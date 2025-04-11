using System;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;
using GymPlusAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymPlusAPI.Infrastructure.Persistence;

public class SpreadsheetRepository : ISpreadsheetRepository
{
    private readonly AppDbContext _context;
    public SpreadsheetRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(Spreadsheet spreadsheet)
    {
        if (spreadsheet == null)
        {
            throw new ArgumentNullException(nameof(spreadsheet));
        }

        await _context.Spreadsheets.AddAsync(spreadsheet);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Spreadsheet spreadsheet)
    {
        if (spreadsheet == null)
        {
            throw new ArgumentNullException(nameof(spreadsheet));
        }

        _context.Spreadsheets.Remove(spreadsheet);
        await _context.SaveChangesAsync();
    }

    public async Task<Spreadsheet?> GetSpreadsheetByIdAsync(int id, Guid userId)
    {
        return await _context.Spreadsheets
            .Include(s => s.Workouts)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
    }

    public async Task<IEnumerable<Spreadsheet>> GetSpreadsheetsByUserAsync(Guid userId)
    {
        return await _context.Spreadsheets
            .Where(s => s.UserId == userId)
            .Include(s => s.Workouts)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(Spreadsheet spreadsheet)
    {
        if (spreadsheet == null)
        {
            throw new ArgumentNullException(nameof(spreadsheet));
        }

        _context.Spreadsheets.Update(spreadsheet);
        await _context.SaveChangesAsync();
    }
}
